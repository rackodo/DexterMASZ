using Discord;
using MASZ.Bot.Services;

namespace MASZ.Bot.Abstractions;

public abstract class Repository
{
	protected Repository(DiscordRest discordRest)
	{
		if (discordRest is not null)
			Identity = discordRest.GetCurrentBotInfo();
	}

	public IUser Identity { get; private set; }

	public void AsUser(IUser currentUser)
	{
		Identity = currentUser;
	}

	public void AsUser(Identity currentUser)
	{
		AsUser(currentUser.GetCurrentUser());
	}
}