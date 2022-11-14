using Bot.Abstractions;
using Bot.Models;
using Bot.Services;

namespace Bot.Data;

public class SettingsRepository : Repository
{
    private readonly BotDatabase _botDatabase;
    private readonly ulong _clientId;

    public SettingsRepository(BotDatabase botDatabase, AppSettings appSettings,
        DiscordRest discordRest) : base(discordRest)
    {
        _botDatabase = botDatabase;
        _clientId = appSettings.ClientId;
    }

    public async Task<AppSettings> GetAppSettings() => await _botDatabase.GetAppSettings(_clientId);

    public async Task AddAppSetting(AppSettings appSettings) => await _botDatabase.AddAppSetting(appSettings);

    public async Task UpdateAppSetting(AppSettings updatedAppSettings) =>
        await _botDatabase.UpdateAppSetting(updatedAppSettings);
}