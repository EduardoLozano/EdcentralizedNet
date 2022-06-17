using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;

namespace EdcentralizedNet.Cache
{
    public interface IApplicationCache
    {
        Task<T> Get<T>(string key) where T : class;
        Task Set<T>(string key, T value, DistributedCacheEntryOptions options) where T : class;
        Task Set<T>(string key, T value, TimeSpan? expireFromNow = null, TimeSpan? expireSliding = null) where T : class;
        Task Remove(string key);
    }
}
