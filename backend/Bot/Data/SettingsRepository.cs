using Bot.Abstractions;
using Bot.Models;
using Bot.Services;

namespace Bot.Data;

public class SettingsRepository(BotDatabase botDatabase, AppSettings appSettings,
    DiscordRest discordRest) : Repository(discordRest)
{
    private readonly BotDatabase _botDatabase = botDatabase;
    private readonly ulong _clientId = appSettings.ClientId;

    public async Task<AppSettings> GetAppSettings() => await _botDatabase.GetAppSettings(_clientId);

    public async Task AddAppSetting(AppSettings appSettings) => await _botDatabase.AddAppSetting(appSettings);

    public async Task UpdateAppSetting(AppSettings updatedAppSettings) =>
        await _botDatabase.UpdateAppSetting(updatedAppSettings);
}
