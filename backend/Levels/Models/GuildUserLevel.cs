using Discord;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Levels.Models;

public class GuildUserLevel
{
	[Key]
	[Column(TypeName = "char(22)")]
	public string Token { get; set; }
	public ulong UserId { get; set; }
	public ulong GuildId { get; set; }
	public long TextXp { get; set; }
	public long VoiceXp { get; set; }
	public long TotalXP => TextXp + VoiceXp;

	protected GuildUserLevel()
	{
		Token = "";
	}

	public GuildUserLevel(IGuildUser guilduser, long textxp = 0, long voicexp = 0)
	{
		UserId = guilduser.Id;
		GuildId = guilduser.GuildId;
		TextXp = textxp;
		VoiceXp = voicexp;
		Token = GenerateToken(guilduser);
	}

	public GuildUserLevel(ulong guildid, ulong userid, long textxp = 0, long voicexp = 0)
	{
		UserId = userid;
		GuildId = guildid;
		TextXp = textxp;
		VoiceXp = voicexp;
		Token = GenerateToken(guildid, userid);
	}

	public static string GenerateToken(IGuildUser user)
	{
		return GenerateToken(user.GuildId, user.Id);
	}

	public static string GenerateToken(ulong guildid, ulong userid)
	{
		var chars = new char[22];
		for (var i = 10; i >= 0; i--)
		{
			chars[i] = (char)('0' + (guildid & 0x3f));
			guildid >>= 6;
		}

		for (var i = 21; i >= 11; i--)
		{
			chars[i] = (char)('0' + (userid & 0x3f));
			userid >>= 6;
		}

		return new string(chars);
	}

	public static int LevelFromXP(long xp, GuildLevelConfig config, out long residualxp, out long levelxp)
	{
		var min = 0;
		var max = 100;
		while (xp > XPFromLevel(max, config))
		{
			min = max;
			max <<= 1;
		}

		var attempts = 200;
		while (attempts-- > 0)
		{
			var middle = (min + max) / 2;

			var xpmiddle = XPFromLevel(middle, config);
			var xpmaxmiddle = XPFromLevel(middle + 1, config);

			if (xp >= xpmaxmiddle)
			{
				min = middle + 1;
			}
			else if (xp < xpmiddle)
			{
				max = middle;
			}
			else
			{
				residualxp = xp - xpmiddle;
				levelxp = xpmaxmiddle - xpmiddle;
				return middle;
			}
		}

		residualxp = -1;
		levelxp = -1;
		return -1;
	}

	public static long XPFromLevel(int level, GuildLevelConfig config)
	{
		float xp = 0;
		long mult = 1;
		foreach (var v in config.Coefficients)
		{
			xp += v * mult;
			mult *= level;
		}
		return (long)Math.Round(xp);
	}
}
