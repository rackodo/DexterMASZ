using Discord;
using Bot.Abstractions;
using Bot.Dynamics;
using Bot.Models;
using Bot.Services;
using System.Diagnostics;

namespace Bot.Data;

public class StatusRepository : Repository, AddAdminStats
{
	private readonly BotDatabase _context;
	private readonly DiscordBot _discordBot;
	private readonly DiscordRest _discordRest;

	public StatusRepository(DiscordBot discordBot, DiscordRest discordRest,
		BotDatabase context) : base(discordRest)
	{
		_discordBot = discordBot;
		_discordRest = discordRest;
		_context = context;
	}

	public async Task AddAdminStatistics(dynamic adminStatus)
	{
		adminStatus.botStatus = GetBotStatus();
		adminStatus.dbStatus = await GetDbStatus();
		adminStatus.cacheStatus = GetCacheStatus();
	}

	public async Task<StatusDetail> GetDbStatus()
	{
		StatusDetail dbStatus = new();
		try
		{
			Stopwatch timer = new();

			timer.Start();
			dbStatus.Online = await _context.Database.CanConnectAsync();
			timer.Stop();

			dbStatus.ResponseTime = timer.Elapsed.TotalMilliseconds;
		}
		catch (Exception)
		{
			dbStatus.Online = false;
		}

		return dbStatus;
	}

	public StatusDetail GetBotStatus()
	{
		StatusDetail botStatus = new();
		try
		{
			botStatus.Online = _discordBot.IsRunning();
			botStatus.LastDisconnect = _discordBot.GetLastDisconnectTime();
			botStatus.ResponseTime = _discordBot.GetLatency();
		}
		catch (Exception)
		{
			botStatus.Online = false;
			botStatus.LastDisconnect = _discordBot.GetLastDisconnectTime();
		}

		return botStatus;
	}

	public StatusDetail GetCacheStatus()
	{
		StatusDetail cacheStatus = new();
		try
		{
			Stopwatch timer = new();

			timer.Start();
			IUser user = _discordRest.GetCurrentBotInfo();
			timer.Stop();

			cacheStatus.ResponseTime = timer.Elapsed.TotalMilliseconds;

			if (user is null)
			{
				cacheStatus.Online = false;
				cacheStatus.Message = "Cache is empty.";
			}
		}
		catch (Exception)
		{
			cacheStatus.Online = false;
		}

		return cacheStatus;
	}
}