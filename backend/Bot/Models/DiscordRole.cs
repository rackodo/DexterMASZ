using Discord;

namespace Bot.Models;

public class DiscordRole
{
    public ulong Id { get; set; }
    public string Name { get; set; }
    public int Color { get; set; }
    public int Position { get; set; }
    public string Permissions { get; set; }

    public DiscordRole(IRole role)
    {
        Id = role.Id;
        Name = role.Name;
        Color = Convert.ToInt32(role.Color.RawValue);
        Position = role.Position;
        Permissions = role.Permissions.GetHashCode().ToString();
    }

    public static DiscordRole GetDiscordRole(IRole role)
    {
        if (role is null)
            return null;
        if (role.Id is 0)
            return null;
        return new DiscordRole(role);
    }
}