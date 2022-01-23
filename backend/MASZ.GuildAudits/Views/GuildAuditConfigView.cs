using MASZ.GuildAudits.Enums;
using MASZ.GuildAudits.Models;

namespace MASZ.GuildAudits.Views;

public class GuildAuditConfigView
{
	public GuildAuditConfigView(GuildAuditConfig config)
	{
		Id = config.Id;
		GuildId = config.GuildId.ToString();
		GuildAuditLogEvent = config.GuildAuditLogEvent;
		ChannelId = config.ChannelId.ToString();
		PingRoles = config.PingRoles.Select(x => x.ToString()).ToArray();
	}

	public int Id { get; set; }
	public string GuildId { get; set; }
	public GuildAuditEvent GuildAuditLogEvent { get; set; }
	public string ChannelId { get; set; }
	public string[] PingRoles { get; set; }
}