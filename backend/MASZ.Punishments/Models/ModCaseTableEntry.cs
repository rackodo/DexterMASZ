using Discord;
using MASZ.Bot.Models;

namespace MASZ.Punishments.Models;

public class ModCaseTableEntry
{
	public ModCaseTableEntry(ModCase modCase, IUser moderator, IUser suspect)
	{
		ModCase = modCase;
		Moderator = new DiscordUser(moderator);
		Suspect = new DiscordUser(suspect);
	}

	public ModCase ModCase { get; set; }
	public DiscordUser Moderator { get; set; }
	public DiscordUser Suspect { get; set; }

	public void RemoveModeratorInfo()
	{
		Moderator = null;
		ModCase.RemoveModeratorInfo();
	}
}