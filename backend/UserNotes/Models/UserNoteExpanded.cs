using Bot.Models;
using Discord;

namespace UserNotes.Models;

public class UserNoteExpanded
{
	public UserNoteExpanded(UserNote userNote, IUser user, IUser moderator)
	{
		UserNote = userNote;
		User = new DiscordUser(user);
		Moderator = new DiscordUser(moderator);
	}

	public UserNote UserNote { get; set; }
	public DiscordUser User { get; set; }
	public DiscordUser Moderator { get; set; }
}