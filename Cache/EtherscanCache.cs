using EdcentralizedNet.EtherscanModels;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EdcentralizedNet.Cache
{
    public class EtherscanCache : IEtherscanCache
    {
        private readonly ILogger<EtherscanCache> _logger;
        private readonly IApplicationCache _cache;

        static string ethTransactionKey = "EthTrx|{0}";

        public EtherscanCache(ILogger<EtherscanCache> logger, IApplicationCache cache)
        {
            _logger = logger;
            _cache = cache;
        }

        public Task<EtherscanTransaction> GetEthTransaction(string transactionHash)
        {
            string key = string.Format(ethTransactionKey, transactionHash);
            return _cache.Get<EtherscanTransaction>(key);
        }

        public Task SetEthTransaction(string transactionHash, EtherscanTransaction transaction)
        {
            string key = string.Format(ethTransactionKey, transactionHash);

            //Expire after it hasnt been accessed for two whole days
            return _cache.Set(key, transaction, expireSliding: new TimeSpan(2, 0, 0, 0));
        }

        public Task RemoveEthTransaction(string transactionHash)
        {
            string key = string.Format(ethTransactionKey, transactionHash);
            return _cache.Remove(key);
        }
    }
}
