using Bot.Services;
using Discord;

namespace Bot.Abstractions;

public abstract class Repository
{
    public IUser Identity { get; private set; }

    protected Repository(DiscordRest discordRest)
    {
        if (discordRest is not null)
            Identity = discordRest.GetCurrentBotInfo();
    }

    public void AsUser(IUser currentUser) => Identity = currentUser;

    public void AsUser(Identity currentUser) => AsUser(currentUser.GetCurrentUser());
}