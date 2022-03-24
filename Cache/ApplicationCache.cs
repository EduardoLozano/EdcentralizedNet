using Microsoft.Extensions.Caching.Distributed;
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
            var cachedUsers = await _cache.GetStringAsync(key);
            return cachedUsers == null ? null : JsonSerializer.Deserialize<T>(cachedUsers);
        }

        public async Task Set<T>(string key, T value, DistributedCacheEntryOptions options) where T : class
        {
            var users = JsonSerializer.Serialize(value);
            await _cache.SetStringAsync(key, users, options);
        }

        public async Task Remove(string key)
        {
            await _cache.RemoveAsync(key);
        }
    }
}
