using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using SolutionName.Application.Abstractions.Services;

namespace SolutionName.Infrastructure.Services;

public class MemoryCacheService : ICacheService
{
    private readonly int _cacheExpirationMinutes; // 30 minutes
    private readonly IMemoryCache _memoryCache;

    public MemoryCacheService(IMemoryCache memoryCache, IConfiguration configuration)
    {
        _memoryCache = memoryCache;
        _cacheExpirationMinutes = configuration.GetValue<int>("CacheExpirationMinutesForCollectionService");
    }

    public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> createFunc, TimeSpan? expiration = null)
    {
        if (_memoryCache.TryGetValue(key, out T result)) return result;

        result = await createFunc();

        var cacheEntryOptions = new MemoryCacheEntryOptions();

        cacheEntryOptions.SetSlidingExpiration(expiration ?? TimeSpan.FromMinutes(_cacheExpirationMinutes));

        _memoryCache.Set(key, result, cacheEntryOptions);

        return result;
    }
}