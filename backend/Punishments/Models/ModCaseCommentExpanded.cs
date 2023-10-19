using Bot.Models;
using Discord;

namespace Punishments.Models;

public class ModCaseCommentExpanded(ModCaseComment comment, IUser commenter)
{
    public ModCaseComment Comment { get; set; } = comment;
    public DiscordUser Commenter { get; set; } = DiscordUser.GetDiscordUser(commenter);

    public void RemoveModeratorInfo(ulong suspectId)
    {
        if (Comment.UserId == suspectId) return;

        Comment.UserId = default;
        Commenter = null;
    }
}
