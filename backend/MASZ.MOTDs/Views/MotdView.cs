using MASZ.MOTDs.Models;

namespace MASZ.MOTDs.Views;

public class MotdView
{
	public MotdView(GuildMotd motd)
	{
		Id = motd.Id;
		GuildId = motd.GuildId.ToString();
		UserId = motd.UserId.ToString();
		CreatedAt = motd.CreatedAt;
		Message = motd.Message;
		ShowMotd = motd.ShowMotd;
	}

	public int Id { get; set; }
	public string GuildId { get; set; }
	public string UserId { get; set; }
	public DateTime CreatedAt { get; set; }
	public string Message { get; set; }
	public bool ShowMotd { get; set; }
}