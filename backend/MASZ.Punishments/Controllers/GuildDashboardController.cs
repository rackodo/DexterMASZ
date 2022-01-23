using MASZ.Bot.Abstractions;
using MASZ.Bot.Enums;
using MASZ.Bot.Services;
using MASZ.Punishments.Data;
using MASZ.Punishments.Views;
using Microsoft.AspNetCore.Mvc;

namespace MASZ.Punishments.Controllers;

[Route("api/v1/guilds/{guildId}/dashboard")]
public class GuildDashboardController : AuthenticatedController
{
	private readonly DiscordRest _discordRest;
	private readonly ModCaseCommentRepository _modCaseCommentRepo;

	public GuildDashboardController(IdentityManager identityManager, ModCaseCommentRepository modCaseCommentRepo,
		DiscordRest discordRest) :
		base(identityManager, modCaseCommentRepo)
	{
		_modCaseCommentRepo = modCaseCommentRepo;
		_discordRest = discordRest;
	}

	[HttpGet("latestcomments")]
	public async Task<IActionResult> LatestComments([FromRoute] ulong guildId)
	{
		var identity = await SetupAuthentication();

		await identity.RequirePermission(DiscordPermission.Moderator, guildId);

		List<CommentExpandedView> view = new();

		foreach (var comment in await _modCaseCommentRepo.GetLastCommentsByGuild(guildId))
			view.Add(new CommentExpandedTableView(
				comment,
				await _discordRest.FetchUserInfo(comment.UserId, CacheBehavior.OnlyCache),
				guildId,
				comment.ModCase.CaseId
			));

		return Ok(view);
	}
}