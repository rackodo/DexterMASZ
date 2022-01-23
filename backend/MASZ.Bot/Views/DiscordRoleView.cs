using Discord;

namespace MASZ.Bot.Views;

public class DiscordRoleView
{
	public DiscordRoleView(IRole role)
	{
		Id = role.Id.ToString();
		Name = role.Name;
		Color = Convert.ToInt32(role.Color.RawValue);
		Position = role.Position;
		Permissions = role.Permissions.GetHashCode().ToString();
	}

	public string Id { get; set; }
	public string Name { get; set; }
	public int Color { get; set; }
	public int Position { get; set; }
	public string Permissions { get; set; }
}