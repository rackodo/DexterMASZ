using System.ComponentModel.DataAnnotations;
using MASZ.AutoMods.Enums;

namespace MASZ.AutoMods.Models;

public class AutoModEvent
{
	[Key] public int Id { get; set; }
	public ulong GuildId { get; set; }
	public AutoModType AutoModType { get; set; }
	public AutoModAction AutoModAction { get; set; }
	public ulong UserId { get; set; }
	public string Username { get; set; }
	public string Nickname { get; set; }
	public string Discriminator { get; set; }
	public ulong MessageId { get; set; }
	public string MessageContent { get; set; }
	public DateTime CreatedAt { get; set; }
	public int? AssociatedCaseId { get; set; }
}