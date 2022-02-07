namespace Bot.Models;

public class CacheKey
{
	private readonly string _key;

	private CacheKey(string key)
	{
		_key = key;
	}

	public string GetValue()
	{
		return _key;
	}

	public static CacheKey User(ulong userId)
	{
		return new CacheKey($"u:{userId}");
	}

	public static CacheKey Guild(ulong guildId)
	{
		return new CacheKey($"g:{guildId}");
	}

	public static CacheKey GuildBans(ulong guildId)
	{
		return new CacheKey($"g:{guildId}:b");
	}

	public static CacheKey GuildBan(ulong guildId, ulong userId)
	{
		return new CacheKey($"g:{guildId}:b:{userId}");
	}

	public static CacheKey GuildUsers(ulong guildId)
	{
		return new CacheKey($"g:{guildId}:m");
	}

	public static CacheKey GuildUser(ulong guildId, ulong userId)
	{
		return new CacheKey($"g:{guildId}:m:{userId}");
	}

	public static CacheKey GuildChannels(ulong guildId)
	{
		return new CacheKey($"g:{guildId}:c");
	}

	public static CacheKey DmChannel(ulong userId)
	{
		return new CacheKey($"c:{userId}");
	}

	public static CacheKey TokenUser(string token)
	{
		return new CacheKey($"t:{token}");
	}

	public static CacheKey TokenUserGuilds(string token)
	{
		return new CacheKey($"t:{token}:g");
	}
}