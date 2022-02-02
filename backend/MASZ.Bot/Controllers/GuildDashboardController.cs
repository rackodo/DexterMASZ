using MASZ.Bot.Abstractions;
using MASZ.Bot.Dynamics;
using MASZ.Bot.Enums;
using MASZ.Bot.Services;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;

namespace MASZ.Bot.Controllers;

[Route("api/v1/guilds/{guildId}/dashboard")]
public class GuildDashboardController : AuthenticatedController
{
	private readonly ServiceCacher _serviceCacher;
	private readonly IServiceProvider _serviceProvider;

	public GuildDashboardController(IServiceProvider serviceProvider, ServiceCacher serviceCacher,
		IdentityManager identityManager) : base(identityManager)
	{
		_serviceProvider = serviceProvider;
		_serviceCacher = serviceCacher;
	}

	[HttpGet("chart")]
	public async Task<IActionResult> GetCharts([FromRoute] ulong guildId, [FromQuery] long? since = null)
	{
		var identity = await SetupAuthentication();

		await identity.RequirePermission(DiscordPermission.Moderator, guildId);

		var sinceTime = DateTime.UtcNow.AddYears(-1);

		if (since != null)
			sinceTime = DateTime.UnixEpoch.AddSeconds(since.Value);

		dynamic chart = new ExpandoObject();

		foreach (var repo in _serviceCacher.GetInitializedAuthenticatedClasses<AddChart>(_serviceProvider, identity))
			await repo.AddChartData(chart, guildId, sinceTime);

		return Ok(chart);
	}

	[HttpGet("stats")]
	public async Task<IActionResult> Stats([FromRoute] ulong guildId)
	{
		var identity = await SetupAuthentication();

		await identity.RequirePermission(DiscordPermission.Moderator, guildId);

		dynamic stats = new ExpandoObject();

		foreach (var repo in _serviceCacher.GetInitializedAuthenticatedClasses<AddGuildStats>(_serviceProvider,
					 identity))
			await repo.AddGuildStatistics(stats, guildId);

		return Ok(stats);
	}

	[HttpGet("search")]
	public async Task<IActionResult> Search([FromRoute] ulong guildId, [FromQuery] string search)
	{
		var identity = await SetupAuthentication();

		await identity.RequirePermission(DiscordPermission.Moderator, guildId);

		if (string.IsNullOrWhiteSpace(search))
			return Ok(new List<string>());

		List<QuickSearchEntry> entries = new();

		foreach (var repo in _serviceCacher.GetInitializedAuthenticatedClasses<AddQuickEntrySearch>(_serviceProvider,
					 identity))
			await repo.AddQuickSearchResults(entries, guildId, search);

		dynamic searchResults = new ExpandoObject();

		searchResults.searchEntries = entries.OrderByDescending(x => x.CreatedAt).ToList();

		foreach (var repo in _serviceCacher.GetInitializedAuthenticatedClasses<AddSearch>(_serviceProvider, identity))
			await repo.AddSearchData(searchResults, guildId, search);

		return Ok(searchResults);
	}
}