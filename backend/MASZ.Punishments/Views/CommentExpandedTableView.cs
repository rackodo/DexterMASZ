using Discord;
using MASZ.Punishments.Models;

namespace MASZ.Punishments.Views;

public class CommentExpandedTableView : CommentExpandedView
{
	public CommentExpandedTableView(ModCaseComment comment, IUser commenter, ulong guildId, int caseId) : base(comment,
		commenter)
	{
		GuildId = guildId.ToString();
		CaseId = caseId;
	}

	public string GuildId { get; set; }
	public int CaseId { get; set; }
}