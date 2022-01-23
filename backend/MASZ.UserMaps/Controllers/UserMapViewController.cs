using MASZ.Bot.Abstractions;
using MASZ.Bot.Enums;
using MASZ.Bot.Services;
using MASZ.UserMaps.Data;
using MASZ.UserMaps.Views;
using Microsoft.AspNetCore.Mvc;

namespace MASZ.UserMaps.Controllers;

[Route("api/v1/guilds/{guildId}/usermapview")]
public class UserMapViewController : AuthenticatedController
{
	private readonly DiscordRest _discordRest;
	private readonly UserMapRepository _userMapRepo;

	public UserMapViewController(IdentityManager identityManager, UserMapRepository userMapRepo,
		DiscordRest discordRest) :
		base(identityManager, userMapRepo)
	{
		_userMapRepo = userMapRepo;
		_discordRest = discordRest;
	}

	[HttpGet]
	public async Task<IActionResult> GetGuildUserNoteView([FromRoute] ulong guildId)
	{
		var identity = await SetupAuthentication();

		await identity.RequirePermission(DiscordPermission.Moderator, guildId);

		var userMaps = await _userMapRepo.GetUserMapsByGuild(guildId);
		List<UserMapExpandedView> userMapsViews = new();

		foreach (var userMap in userMaps)
			userMapsViews.Add(new UserMapExpandedView(
				userMap,
				await _discordRest.FetchUserInfo(userMap.UserA, CacheBehavior.OnlyCache),
				await _discordRest.FetchUserInfo(userMap.UserB, CacheBehavior.OnlyCache),
				await _discordRest.FetchUserInfo(userMap.CreatorUserId, CacheBehavior.OnlyCache)
			));

		return Ok(userMapsViews);
	}
}