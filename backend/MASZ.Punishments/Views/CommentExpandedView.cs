using Discord;
using MASZ.Bot.Views;
using MASZ.Punishments.Models;

namespace MASZ.Punishments.Views;

public class CommentExpandedView
{
	public CommentExpandedView(ModCaseComment comment, IUser commenter)
	{
		Comment = new CommentsView(comment);
		Commenter = new DiscordUserView(commenter);
	}

	public CommentsView Comment { get; set; }
	public DiscordUserView Commenter { get; set; }

	public void RemoveModeratorInfo(string suspectId)
	{
		if (Comment.UserId == suspectId) return;

		Comment.UserId = null;
		Commenter = null;
	}
}