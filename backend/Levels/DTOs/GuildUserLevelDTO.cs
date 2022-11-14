using Bot.Models;

namespace Levels.DTOs;

public class GuildUserLevelDTO
{
    public ulong GuildId { get; set; }
    public ulong UserId { get; set; }
    public ExperienceRecordDTO TextXp { get; set; }
    public ExperienceRecordDTO VoiceXp { get; set; }
    public ExperienceRecordDTO TotalXp { get; set; }
    public DiscordUser User { get; set; }

    public GuildUserLevelDTO(ulong guildId, ulong userId, ExperienceRecordDTO textXp, ExperienceRecordDTO voiceXp,
        ExperienceRecordDTO totalXp, DiscordUser user)
    {
        GuildId = guildId;
        UserId = userId;
        TextXp = textXp;
        VoiceXp = voiceXp;
        TotalXp = totalXp;
        User = user;
    }
}