using MASZ.UserMaps.Models;

namespace MASZ.UserMaps.Views;

public class UserMapView
{
	public UserMapView(UserMap userMaps)
	{
		Id = userMaps.Id;
		GuildId = userMaps.GuildId.ToString();
		UserA = userMaps.UserA.ToString();
		UserB = userMaps.UserB.ToString();
		Reason = userMaps.Reason;
		CreatedAt = userMaps.CreatedAt;
		CreatorUserId = userMaps.CreatorUserId.ToString();
	}

	public int Id { get; set; }
	public string GuildId { get; set; }
	public string UserA { get; set; }
	public string UserB { get; set; }
	public string Reason { get; set; }
	public DateTime CreatedAt { get; set; }
	public string CreatorUserId { get; set; }
}