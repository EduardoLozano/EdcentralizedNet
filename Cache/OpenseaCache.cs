using EdcentralizedNet.OpenseaModels;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EdcentralizedNet.Cache
{
    public class OpenseaCache : IOpenseaCache
    {
        private readonly ILogger<OpenseaCache> _logger;
        private readonly IApplicationCache _cache;

        static string collectionStatsKey = "Stats|{0}";
        static string mintAssetEventKey = "MintEvent|{0}{1}";

        public OpenseaCache(ILogger<OpenseaCache> logger, IApplicationCache cache)
        {
            _logger = logger;
            _cache = cache;
        }

        #region Collection Stats
        public Task<OSStats> GetStatsForCollection(string collectionSlug)
        {
            string key = string.Format(collectionStatsKey, collectionSlug);
            return _cache.Get<OSStats>(key);
        }

        public Task SetStatsForCollection(string collectionSlug, OSStats stats)
        {
            //Default expiration to 30 minutes
            TimeSpan expiration = new TimeSpan(0, 30, 0);
            string key = string.Format(collectionStatsKey, collectionSlug);
            
            //For opensea stats, if we dont receive any back, lets store an empty object
            //Lets also store it for a whole day so we dont continue hitting the api for this collection
            if(stats == null)
            {
                stats = new OSStats();
                expiration = new TimeSpan(1, 0, 0, 0);
            }

            return _cache.Set<OSStats>(key, stats, expireFromNow: expiration); 
        }

        public Task RemoveStatsForCollection(string collectionSlug)
        {
            string key = string.Format(collectionStatsKey, collectionSlug);
            return _cache.Remove(key);
        }
        #endregion

        #region Mint Asset Event
        public Task<OSEvent> GetAssetMintEvent(string contractAddress, string tokenId)
        {
            string key = string.Format(mintAssetEventKey, contractAddress, tokenId);
            return _cache.Get<OSEvent>(key);
        }

        public Task SetAssetMintEvent(string contractAddress, string tokenId, OSEvent aEvent)
        {
            string key = string.Format(mintAssetEventKey, contractAddress, tokenId);

            //Expire after it hasnt been accessed for a week
            return _cache.Set(key, aEvent, expireSliding: new TimeSpan(7,0,0,0));
        }

        public Task RemoveAssetMintEvent(string contractAddress, string tokenId)
        {
            string key = string.Format(mintAssetEventKey, contractAddress, tokenId);
            return _cache.Remove(key);
        }
        #endregion
    }
}
