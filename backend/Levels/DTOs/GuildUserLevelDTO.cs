using Bot.Models;

namespace Levels.DTOs;

public class GuildUserLevelDto
{
    public ulong GuildId { get; set; }
    public ulong UserId { get; set; }
    public ExperienceRecordDto TextXp { get; set; }
    public ExperienceRecordDto VoiceXp { get; set; }
    public ExperienceRecordDto TotalXp { get; set; }
    public DiscordUser User { get; set; }

    public GuildUserLevelDto(ulong guildId, ulong userId, ExperienceRecordDto textXp, ExperienceRecordDto voiceXp,
        ExperienceRecordDto totalXp, DiscordUser user)
    {
        GuildId = guildId;
        UserId = userId;
        TextXp = textXp;
        VoiceXp = voiceXp;
        TotalXp = totalXp;
        User = user;
    }
}
