using System.ComponentModel.DataAnnotations;

namespace MASZ.Bot.Models;

public class ApiToken
{
	[Key] public int Id { get; set; }
	public string Name { get; set; }
	public byte[] TokenSalt { get; set; }
	public byte[] TokenHash { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime ValidUntil { get; set; }
}