using Discord;

namespace MASZ.Punishments.Models;

public class ModCaseCommentExpandedTable : ModCaseCommentExpanded
{
	public ModCaseCommentExpandedTable(ModCaseComment comment, IUser commenter, ulong guildId, int caseId) : base(comment,
		commenter)
	{
		GuildId = guildId.ToString();
		CaseId = caseId;
	}

	public string GuildId { get; set; }
	public int CaseId { get; set; }
}