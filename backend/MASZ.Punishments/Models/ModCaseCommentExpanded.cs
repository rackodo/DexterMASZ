using Discord;
using MASZ.Bot.Models;

namespace MASZ.Punishments.Models;

public class ModCaseCommentExpanded
{
	public ModCaseCommentExpanded(ModCaseComment comment, IUser commenter)
	{
		Comment = comment;
		Commenter = new DiscordUser(commenter);
	}

	public ModCaseComment Comment { get; set; }
	public DiscordUser Commenter { get; set; }

	public void RemoveModeratorInfo(ulong suspectId)
	{
		if (Comment.UserId == suspectId) return;

		Comment.UserId = default;
		Commenter = null;
	}
}