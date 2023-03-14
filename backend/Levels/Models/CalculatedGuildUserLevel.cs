using Bot.Models;
using Levels.DTOs;

namespace Levels.Models;

public class CalculatedGuildUserLevel : GuildUserLevel
{
    private LevelData _textLevel;
    private LevelData _totalLevel;
    private LevelData _voiceLevel;

    public CalculatedGuildUserLevel(GuildUserLevel guildUserLevel, GuildLevelConfig config = null)
    {
        Token = guildUserLevel.Token;
        UserId = guildUserLevel.UserId;
        GuildId = guildUserLevel.GuildId;
        TextXp = guildUserLevel.TextXp;
        VoiceXp = guildUserLevel.VoiceXp;
        Config = config;
    }

    public GuildLevelConfig Config { get; set; }

    public new long TextXp
    {
        get => base.TextXp;
        set
        {
            _textLevel?.SetXp(value, Config!);
            _totalLevel?.SetXp(value + base.VoiceXp, Config!);
            base.TextXp = value;
        }
    }

    public new long VoiceXp
    {
        get => base.VoiceXp;
        set
        {
            _voiceLevel?.SetXp(value, Config!);
            _totalLevel?.SetXp(value + base.TextXp, Config!);
            base.VoiceXp = value;
        }
    }

    public LevelData Text
    {
        get
        {
            _textLevel ??= LevelFromXp(TextXp);
            return _textLevel;
        }
    }

    public LevelData Voice
    {
        get
        {
            _voiceLevel ??= LevelFromXp(VoiceXp);
            return _voiceLevel;
        }
    }

    public LevelData Total
    {
        get
        {
            _totalLevel ??= LevelFromXp(TextXp + VoiceXp);
            return _totalLevel;
        }
    }

    public LevelData LevelFromXp(long xp) => Config is null
        ? throw new NullReferenceException("_config is null. Unable to get level from XP before running SetConfig")
        : new LevelData(xp, Config);

    public long XpFromLevel(int level) => Config is null
        ? throw new NullReferenceException("_config is null. Unable to get XP from level before running SetConfig")
        : XpFromLevel(level, Config);

    public GuildUserLevelDto ToDto(DiscordUser user) =>
        new(GuildId, UserId, Text.ToDto(), Voice.ToDto(), Total.ToDto(), user);
}
