using Discord;

namespace Punishments.Models;

public class ModCaseCommentExpandedTable(ModCaseComment comment, IUser commenter, ulong guildId, int caseId) : ModCaseCommentExpanded(
    comment,
    commenter)
{
    public ulong GuildId { get; set; } = guildId;
    public int CaseId { get; set; } = caseId;
}
