using Bot.Abstractions;
using Bot.Dynamics;
using Bot.Enums;
using Bot.Services;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;

namespace Bot.Controllers;

[Route("api/v1/guilds/{guildId}/dashboard")]
public class GuildDashboardController(IServiceProvider serviceProvider, CachedServices cachedServices,
    IdentityManager identityManager) : AuthenticatedController(identityManager)
{
    private readonly CachedServices _cachedServices = cachedServices;
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    [HttpGet("chart")]
    public async Task<IActionResult> GetCharts([FromRoute] ulong guildId, [FromQuery] long? since = null)
    {
        var identity = await SetupAuthentication();

        await identity.RequirePermission(DiscordPermission.Moderator, guildId);

        var sinceTime = DateTime.UtcNow.AddYears(-1);

        if (since != null)
            sinceTime = DateTime.UnixEpoch.AddSeconds(since.Value);

        dynamic chart = new ExpandoObject();

        foreach (var repo in _cachedServices.GetInitializedAuthenticatedClasses<IAddChart>(_serviceProvider, identity))
            await repo.AddChartData(chart, guildId, sinceTime);

        return Ok(chart);
    }

    [HttpGet("stats")]
    public async Task<IActionResult> Stats([FromRoute] ulong guildId)
    {
        var identity = await SetupAuthentication();

        await identity.RequirePermission(DiscordPermission.Moderator, guildId);

        dynamic stats = new ExpandoObject();

        foreach (var repo in _cachedServices.GetInitializedAuthenticatedClasses<IAddGuildStats>(_serviceProvider,
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

        List<IQuickSearchEntry> entries = [];

        foreach (var repo in _cachedServices.GetInitializedAuthenticatedClasses<IAddQuickEntrySearch>(_serviceProvider,
                     identity))
            await repo.AddQuickSearchResults(entries, guildId, search);

        dynamic searchResults = new ExpandoObject();

        searchResults.searchEntries = entries.OrderByDescending(x => x.CreatedAt).ToList();

        foreach (var repo in _cachedServices.GetInitializedAuthenticatedClasses<IAddSearch>(_serviceProvider, identity))
            await repo.AddSearchData(searchResults, guildId, search);

        return Ok(searchResults);
    }
}
