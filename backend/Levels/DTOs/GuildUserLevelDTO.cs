using Bot.Models;

namespace Levels.DTOs;

public class GuildUserLevelDto(ulong guildId, ulong userId, ExperienceRecordDto textXp, ExperienceRecordDto voiceXp,
    ExperienceRecordDto totalXp, DiscordUser user)
{
    public ulong GuildId { get; set; } = guildId;
    public ulong UserId { get; set; } = userId;
    public ExperienceRecordDto TextXp { get; set; } = textXp;
    public ExperienceRecordDto VoiceXp { get; set; } = voiceXp;
    public ExperienceRecordDto TotalXp { get; set; } = totalXp;
    public DiscordUser User { get; set; } = user;
}
