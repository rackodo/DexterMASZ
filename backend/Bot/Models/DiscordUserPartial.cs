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
        Discriminator = user.Discriminator == "0000" ? string.Empty : user.Discriminator;
    }

    public static DiscordUserPartial GetPartialDiscordUser(IUser user) => user is null ? null : user.Id is 0 ? null : new DiscordUserPartial(user);
}
