using MASZ.Bot.Abstractions;
using MASZ.Bot.Enums;

namespace MASZ.Bot.Exceptions;

public class NotFoundInCacheException : ApiException
{
	public NotFoundInCacheException(string cacheKey) : base($"'{cacheKey}' is not cached.", ApiError.NotFoundInCache)
	{
		CacheKey = cacheKey;
	}

	public NotFoundInCacheException() : base("Failed to find key in local cache.", ApiError.NotFoundInCache)
	{
	}

	public string CacheKey { get; set; }
}