using EdcentralizedNet.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdcentralizedNet.Cache
{
    public class OpenseaCache : IOpenseaCache
    {
        private readonly ILogger<OpenseaCache> _logger;
        private readonly IApplicationCache _cache;
        private readonly DistributedCacheEntryOptions _cacheOptions;

        static string collectionStatsKey = "Stats|{0}";

        public OpenseaCache(ILogger<OpenseaCache> logger, IApplicationCache cache)
        {
            _logger = logger;
            _cache = cache;
            _cacheOptions = new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = new TimeSpan(0, 10, 0) };
        }

        #region Collection Stats
        public Task<OSStats> GetStatsForCollection(string collectionSlug)
        {
            string key = string.Format(collectionStatsKey, collectionSlug);
            return _cache.Get<OSStats>(key);
        }

        public Task SetStatsForCollection(string collectionSlug, OSStats stats)
        {
            string key = string.Format(collectionStatsKey, collectionSlug);
            return _cache.Set<OSStats>(key, stats, _cacheOptions);
        }

        public Task RemoveStatsForCollection(string collectionSlug)
        {
            string key = string.Format(collectionStatsKey, collectionSlug);
            return _cache.Get<OSStats>(key);
        }
        #endregion
    }
}
