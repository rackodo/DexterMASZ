using Bot.Abstractions;
using Bot.Dynamics;
using Bot.Services;
using Music.Models;

namespace Music.Data;

public class StartRepository : Repository, IDeleteGuildData
{
    private readonly MusicDatabase _musicDatabase;

    public StartRepository(DiscordRest discordRest, MusicDatabase musicDatabase) : base(discordRest) =>
        _musicDatabase = musicDatabase;

    public async Task DeleteGuildData(ulong guildId) => await _musicDatabase.DeleteGuildData(guildId);

    public async Task SetGuildStartTime(ulong guildId, DateTime startTime) =>
        await _musicDatabase.SetGuildStartTime(guildId, startTime);

    public async Task<StartTime> GetGuildStartTime(ulong guildId) => await _musicDatabase.GetGuildStartTime(guildId);
}
