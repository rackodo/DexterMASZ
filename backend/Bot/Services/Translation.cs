using Bot.Abstractions;
using Bot.Data;
using Bot.Enums;
using Bot.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Bot.Services;

public class Translation
{
    private readonly IServiceProvider _serviceProvider;

    private Language? _language;

    public Translation(SettingsRepository settingsRepository, IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        if (_language is not null) return;

        var config = settingsRepository.GetAppSettings().GetAwaiter().GetResult();

        _language = config.DefaultLanguage;
    }

    public void SetLanguage(Language? language)
    {
        if (language != null)
            _language = language.Value;
    }

    public async Task SetLanguage(ulong guildId)
    {
        var guildConfig = await _serviceProvider
            .GetRequiredService<GuildConfigRepository>().GetGuildConfig(guildId);

        SetLanguage(guildConfig);
    }

    public void SetLanguage(GuildConfig guildConfig)
    {
        if (guildConfig != null)
            SetLanguage(guildConfig.PreferredLanguage);
    }

    public T Get<T>()
    {
        var translator = _serviceProvider.GetRequiredService<T>();

        if (_language != null) (translator as Translator)!.PreferredLanguage = _language.Value;

        return translator;
    }
}