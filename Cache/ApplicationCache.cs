using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace EdcentralizedNet.Cache
{
    public class ApplicationCache : IApplicationCache
    {
        private readonly IDistributedCache _cache;

        public ApplicationCache(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T> Get<T>(string key) where T : class
        {
            var cachedEntity = await _cache.GetStringAsync(key);
            return cachedEntity == null ? null : JsonSerializer.Deserialize<T>(cachedEntity);
        }

        public async Task Set<T>(string key, T value, TimeSpan? expireFromNow = null, TimeSpan? expireSliding = null) where T : class
        {
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = expireFromNow,
                SlidingExpiration = expireSliding
            };

            await Set<T>(key, value, options);
        }

        public async Task Set<T>(string key, T value, DistributedCacheEntryOptions options = null) where T : class
        {
            var entity = JsonSerializer.Serialize(value);
            await _cache.SetStringAsync(key, entity, options);
        }

        public async Task Remove(string key)
        {
            await _cache.RemoveAsync(key);
        }
    }
}
