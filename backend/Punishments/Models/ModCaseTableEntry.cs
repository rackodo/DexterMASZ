using Bot.Models;
using Discord;

namespace Punishments.Models;

public class ModCaseTableEntry(ModCase modCase, IUser moderator, IUser suspect)
{
    public ModCase ModCase { get; set; } = modCase;
    public DiscordUser Moderator { get; set; } = DiscordUser.GetDiscordUser(moderator);
    public DiscordUser Suspect { get; set; } = DiscordUser.GetDiscordUser(suspect);

    public void RemoveModeratorInfo()
    {
        Moderator = null;
        ModCase.RemoveModeratorInfo();
    }
}
