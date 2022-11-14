using Bot.Models;
using Discord;

namespace Punishments.Models;

public class ModCaseTableEntry
{
    public ModCase ModCase { get; set; }
    public DiscordUser Moderator { get; set; }
    public DiscordUser Suspect { get; set; }

    public ModCaseTableEntry(ModCase modCase, IUser moderator, IUser suspect)
    {
        ModCase = modCase;
        Moderator = DiscordUser.GetDiscordUser(moderator);
        Suspect = DiscordUser.GetDiscordUser(suspect);
    }

    public void RemoveModeratorInfo()
    {
        Moderator = null;
        ModCase.RemoveModeratorInfo();
    }
}