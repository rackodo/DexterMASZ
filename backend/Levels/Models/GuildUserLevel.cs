using Discord;
using Microsoft.EntityFrameworkCore;

namespace Levels.Models;

[PrimaryKey(nameof(GuildId), nameof(UserId))]
public class GuildUserLevel
{
    public ulong UserId { get; set; }
    public ulong GuildId { get; set; }
    public long TextXp { get; set; }
    public long VoiceXp { get; set; }
    public long TotalXp => TextXp + VoiceXp;

    protected GuildUserLevel() { }

    public GuildUserLevel(IGuildUser guildUser, long textXp = 0, long voiceXp = 0)
    {
        UserId = guildUser.Id;
        GuildId = guildUser.GuildId;
        TextXp = textXp;
        VoiceXp = voiceXp;
    }

    public GuildUserLevel(ulong guildId, ulong userId, long textXp = 0, long voiceXp = 0)
    {
        UserId = userId;
        GuildId = guildId;
        TextXp = textXp;
        VoiceXp = voiceXp;
    }

    public static int LevelFromXp(long xp, GuildLevelConfig config, out long residualXp, out long levelXp)
    {
        var min = 0;
        var max = 100;
        while (xp > XpFromLevel(max, config))
        {
            min = max;
            max <<= 1;
        }

        var attempts = 200;
        while (attempts-- > 0)
        {
            var middle = (min + max) / 2;

            var xpMiddle = XpFromLevel(middle, config);
            var xpMaxMiddle = XpFromLevel(middle + 1, config);

            if (xp >= xpMaxMiddle)
            {
                min = middle + 1;
            }
            else if (xp < xpMiddle)
            {
                max = middle;
            }
            else
            {
                residualXp = xp - xpMiddle;
                levelXp = xpMaxMiddle - xpMiddle;
                return middle;
            }
        }

        residualXp = -1;
        levelXp = -1;
        return -1;
    }

    public static long XpFromLevel(int level, GuildLevelConfig config)
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
