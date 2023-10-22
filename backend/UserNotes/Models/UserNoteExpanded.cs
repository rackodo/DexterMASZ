using Bot.Models;
using Discord;

namespace UserNotes.Models;

public class UserNoteExpanded(UserNote userNote, IUser user, IUser moderator)
{
    public UserNote UserNote { get; set; } = userNote;
    public DiscordUser User { get; set; } = DiscordUser.GetDiscordUser(user);
    public DiscordUser Moderator { get; set; } = DiscordUser.GetDiscordUser(moderator);
}
