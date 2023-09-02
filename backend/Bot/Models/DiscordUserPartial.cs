using Discord;

namespace Bot.Models;

public class DiscordUserPartial
{
    public ulong Id { get; set; }
    public string Username { get; set; }

    protected DiscordUserPartial(IUser user)
    {
        Id = user.Id;
        Username = user.Discriminator == "0000" ? user.Username : $"{user.Username}";
    }

    public static DiscordUserPartial GetPartialDiscordUser(IUser user) => user is null ? null : user.Id is 0 ? null : new DiscordUserPartial(user);
}
