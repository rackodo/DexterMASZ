using Discord;

namespace Bot.Models;

public class DiscordRole(IRole role)
{
    public ulong Id { get; set; } = role.Id;
    public string Name { get; set; } = role.Name;
    public int Color { get; set; } = Convert.ToInt32(role.Color.RawValue);
    public int Position { get; set; } = role.Position;
    public string Permissions { get; set; } = role.Permissions.GetHashCode().ToString();

    public static DiscordRole GetDiscordRole(IRole role) => role is null ? null : role.Id is 0 ? null : new DiscordRole(role);
}
