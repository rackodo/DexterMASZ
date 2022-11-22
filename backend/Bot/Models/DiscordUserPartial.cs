using Discord;

namespace Bot.Models;

public class DiscordUserPartial
{
    public ulong Id { get; set; }
    public string Username { get; set; }
    public string Discriminator { get; set; }

    protected DiscordUserPartial(IUser user)
    {
        Id = user.Id;
        Username = user.Username;
        Discriminator = user.Discriminator;
    }

    public static DiscordUserPartial GetPartialDiscordUser(IUser user)
    {
        if (user is null)
            return null;
        if (user.Id is 0)
            return null;
        return new DiscordUserPartial(user);
    }
}
