using System;
using System.Threading.Tasks;
using ApiRequestManager.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace ApiRequestManager.Services
{
    public class CacheService : IMemoryCacheService
    {
        private readonly int SLIDING_EXPIRATION_SECONDS = 300;
        private IMemoryCache _cache;

        public CacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public T Get<T>(string key)
        {
            return default(T);
        }

        public async Task<T> GetAsync<T>(string key)
        {
            if (_cache.TryGetValue(key, out T cacheEntry))
            {
                return await Task.FromResult<T>(cacheEntry);
            }

            return await Task.FromResult<T>(default(T));
        }

        public void Set<T>(string key, object entry)
        {

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                            // Set cache entry size by extension method.
                            .SetSize(1)
                            // Keep in cache for this time, reset time if accessed.
                            .SetSlidingExpiration(TimeSpan.FromSeconds(SLIDING_EXPIRATION_SECONDS));

            // Set cache entry size via property.
            // cacheEntryOptions.Size = 1;

            // Save data in cache.
            _cache.Set(key, entry, cacheEntryOptions);

        }

        public void Remove<T>(string key)
        {
            _cache.Remove(key);
        }

    }
}