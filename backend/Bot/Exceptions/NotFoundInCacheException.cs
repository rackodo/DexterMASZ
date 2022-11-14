using Bot.Abstractions;
using Bot.Enums;

namespace Bot.Exceptions;

public class NotFoundInCacheException : ApiException
{
    public string CacheKey { get; set; }

    public NotFoundInCacheException(string cacheKey) : base($"'{cacheKey}' is not cached.", ApiError.NotFoundInCache) =>
        CacheKey = cacheKey;

    public NotFoundInCacheException() : base("Failed to find key in local cache.", ApiError.NotFoundInCache)
    {
    }
}