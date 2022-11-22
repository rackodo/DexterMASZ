using Discord;

namespace Punishments.Models;

public class ModCaseCommentExpandedTable : ModCaseCommentExpanded
{
    public ulong GuildId { get; set; }
    public int CaseId { get; set; }

    public ModCaseCommentExpandedTable(ModCaseComment comment, IUser commenter, ulong guildId, int caseId) : base(
        comment,
        commenter)
    {
        GuildId = guildId;
        CaseId = caseId;
    }
}
