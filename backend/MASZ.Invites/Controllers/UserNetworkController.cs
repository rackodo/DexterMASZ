using System.ComponentModel.DataAnnotations;
using MASZ.Bot.Abstractions;
using MASZ.Bot.Enums;
using MASZ.Bot.Models;
using MASZ.Bot.Services;
using MASZ.Invites.Data;
using MASZ.Invites.Models;
using Microsoft.AspNetCore.Mvc;

namespace MASZ.Invites.Controllers;

[Route("api/v1/network")]
public class UserNetworkController : AuthenticatedController
{
	private readonly DiscordRest _discordRest;
	private readonly InviteRepository _inviteRepository;

	public UserNetworkController(IdentityManager identityManager, DiscordRest discordRest,
		InviteRepository inviteRepository) : base(identityManager, inviteRepository)
	{
		_discordRest = discordRest;
		_inviteRepository = inviteRepository;
	}

	[HttpGet("invite")]
	public async Task<IActionResult> GetInviteNetwork([FromQuery] [Required] string inviteUrl)
	{
		var identity = await SetupAuthentication();

		var invites = await _inviteRepository.GetInvitesByCode(inviteUrl);

		if (invites == null || invites.Count == 0)
			return NotFound();

		await identity.RequirePermission(DiscordPermission.Moderator, invites[0].GuildId);

		DiscordGuild guild = new(_discordRest.FetchGuildInfo(invites[0].GuildId, CacheBehavior.Default));

		List<UserInviteExpanded> inviteViews = new();

		foreach (var invite in invites)
			inviteViews.Add(new UserInviteExpanded(
				invite,
				await _discordRest.FetchUserInfo(invite.JoinedUserId, CacheBehavior.OnlyCache),
				await _discordRest.FetchUserInfo(invite.InviteIssuerId, CacheBehavior.OnlyCache)
			));

		return Ok(new
		{
			invites = inviteViews,
			guild
		});
	}
}