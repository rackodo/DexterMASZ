using MASZ.Bot.Abstractions;
using MASZ.Bot.Models;
using MASZ.Bot.Services;

namespace MASZ.Bot.Data;

public class SettingsRepository : Repository
{
	private readonly BotDatabase _botDatabase;

	public SettingsRepository(BotDatabase botDatabase, DiscordRest discordRest) : base(discordRest)
	{
		_botDatabase = botDatabase;
	}

	public async Task<AppSettings> GetAppSettings()
	{
		return await _botDatabase.GetAppSettings();
	}

	public async Task AddAppSetting(AppSettings appSettings)
	{
		await _botDatabase.AddAppSetting(appSettings);
	}
	
	public async Task UpdateAppSetting(AppSettings updatedAppSettings)
	{
		await _botDatabase.UpdateAppSetting(updatedAppSettings);
	}
}