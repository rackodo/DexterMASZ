using MOTDs.Models;

namespace MOTDs.Views;

public class MotdView(GuildMotd motd)
{
    public int Id { get; set; } = motd.Id;
    public string GuildId { get; set; } = motd.GuildId.ToString();
    public string UserId { get; set; } = motd.UserId.ToString();
    public DateTime CreatedAt { get; set; } = motd.CreatedAt;
    public string Message { get; set; } = motd.Message;
    public bool ShowMotd { get; set; } = motd.ShowMotd;
}
