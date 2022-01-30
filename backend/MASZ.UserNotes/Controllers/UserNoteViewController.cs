using MASZ.Bot.Abstractions;
using MASZ.Bot.Enums;
using MASZ.Bot.Services;
using MASZ.UserNotes.Data;
using MASZ.UserNotes.Models;
using Microsoft.AspNetCore.Mvc;

namespace MASZ.UserNotes.Controllers;

[Route("api/v1/guilds/{guildId}/usernoteview")]
public class UserNoteViewController : AuthenticatedController
{
	private readonly DiscordRest _discordRest;
	private readonly UserNoteRepository _userNoteRepo;

	public UserNoteViewController(IdentityManager identityManager, UserNoteRepository userNoteRepo,
		DiscordRest discordRest) :
		base(identityManager, userNoteRepo)
	{
		_userNoteRepo = userNoteRepo;
		_discordRest = discordRest;
	}

	[HttpGet]
	public async Task<IActionResult> GetGuildUserNoteView([FromRoute] ulong guildId)
	{
		var identity = await SetupAuthentication();

		await identity.RequirePermission(DiscordPermission.Moderator, guildId);

		var userNotes = await _userNoteRepo.GetUserNotesByGuild(guildId);
		List<UserNoteExpanded> userNoteViews = new();

		foreach (var userNote in userNotes)
			userNoteViews.Add(new UserNoteExpanded(
				userNote,
				await _discordRest.FetchUserInfo(userNote.UserId, CacheBehavior.OnlyCache),
				await _discordRest.FetchUserInfo(userNote.CreatorId, CacheBehavior.OnlyCache)
			));

		return Ok(userNoteViews);
	}
}