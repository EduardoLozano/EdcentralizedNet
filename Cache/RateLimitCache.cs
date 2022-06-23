using StackExchange.Redis;
using System;

namespace EdcentralizedNet.Cache
{
    public class RateLimitCache : IRateLimitCache
    {
        private readonly IDatabase _cache;

        private readonly string _resetTimeKey = "Edcentralized|{0}|LastResetTime";
        private readonly string _requestCounterKey = "Edcentralized|{0}|RequestCounter";

        public RateLimitCache(IConnectionMultiplexer connectionMultiplexer)
        {
            _cache = connectionMultiplexer.GetDatabase();
        }

        public bool CanRequestOpensea()
        {
            //Opensea currently limits to 4 requests per second (1000 ms)
            string key = "OpenseaApi";
            long rateInterval = 1000;
            int rateLimit = 4;

            return CanRequestBasedOnRateLimit(key, rateInterval, rateLimit);
        }

        public bool CanRequestEtherscan()
        {
            //Etherscan currently limits to 5 requests per second and 100K calls a day (1000 ms)
            string key = "EtherscanApi";
            long rateInterval = 1000;
            int rateLimit = 5;

            return CanRequestBasedOnRateLimit(key, rateInterval, rateLimit);
        }

        private bool CanRequestBasedOnRateLimit(string resourceKey, long rateInterval, int rateLimit)
        {
            string requestCounterKey = string.Format(_requestCounterKey, resourceKey);
            string resetTimeKey = string.Format(_resetTimeKey, resourceKey);

            //Try and get the last reset time
            string resetTimeStr = _cache.StringGet(resetTimeKey);
            long currTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            long resetTime = 0;

            //If this was the first request or we have elapsed the time interval, reset the request counter to full amount
            if (!long.TryParse(resetTimeStr, out resetTime) || currTime - resetTime >= rateInterval)
            {
                _cache.StringSet(requestCounterKey, rateLimit);
            }
            else
            {
                //Try and get the request counter to see if we have any requests available
                string requestCounterStr = _cache.StringGet(requestCounterKey);
                int requestCounter = 0;

                if (int.TryParse(requestCounterStr, out requestCounter) && requestCounter <= 0)
                {
                    return false;
                }
            }

            //If we got to this point, we are allowed to make a request
            //Set new reset time and decrement counter
            _cache.StringSet(resetTimeKey, currTime);
            _cache.StringDecrement(requestCounterKey);

            return true;
        }
    }
}
