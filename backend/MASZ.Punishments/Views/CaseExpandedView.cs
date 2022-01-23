using Discord;
using MASZ.Bot.Views;
using MASZ.Punishments.Models;
using MASZ.UserNotes.Views;

namespace MASZ.Punishments.Views;

public class CaseExpandedView
{
	public CaseExpandedView(ModCase modCase, IUser moderator, IUser lastModerator, IUser suspect,
		List<CommentExpandedView> comments, UserNoteExpandedView userNoteView)
	{
		ModCase = new CaseView(modCase);
		Moderator = new DiscordUserView(moderator);
		LastModerator = new DiscordUserView(lastModerator);
		Suspect = new DiscordUserView(suspect);
		Comments = comments;
		UserNote = userNoteView;

		if (modCase.PunishedUntil == null) return;

		if (!(modCase.PunishedUntil > modCase.CreatedAt)) return;

		if (modCase.PunishedUntil < DateTime.UtcNow)
		{
			PunishmentProgress = 100;
		}
		else
		{
			var totalPunished = (modCase.PunishedUntil.Value - modCase.CreatedAt).TotalSeconds;
			var alreadyPunished = (DateTime.UtcNow - modCase.CreatedAt).TotalSeconds;

			PunishmentProgress = alreadyPunished / totalPunished * 100;
		}
	}

	public CaseView ModCase { get; set; }
	public DiscordUserView Moderator { get; set; }
	public DiscordUserView LastModerator { get; set; }
	public DiscordUserView Suspect { get; set; }
	public DiscordUserView LockedBy { get; set; }
	public DiscordUserView DeletedBy { get; set; }
	public List<CommentExpandedView> Comments { get; set; }
	public UserNoteExpandedView UserNote { get; set; }
	public double? PunishmentProgress { get; set; }

	public void RemoveModeratorInfo()
	{
		Moderator = null;
		LastModerator = null;
		LockedBy = null;
		DeletedBy = null;
		ModCase.RemoveModeratorInfo();

		foreach (var comment in Comments)
			comment.RemoveModeratorInfo(ModCase.UserId);
	}
}