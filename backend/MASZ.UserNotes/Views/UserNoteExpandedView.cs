using Discord;
using MASZ.Bot.Views;
using MASZ.UserNotes.Models;

namespace MASZ.UserNotes.Views;

public class UserNoteExpandedView
{
	public UserNoteExpandedView(UserNote userNote, IUser user, IUser moderator)
	{
		UserNote = new UserNoteView(userNote);
		User = new DiscordUserView(user);
		Moderator = new DiscordUserView(moderator);
	}

	public UserNoteView UserNote { get; set; }
	public DiscordUserView User { get; set; }
	public DiscordUserView Moderator { get; set; }
}