using Bot.Abstractions;
using Bot.Services;
using Discord;
using Levels.Models;

namespace Levels.Data;

public class GuildUserLevelRepository : Repository
{
    private readonly LevelsDatabase _database;

    public GuildUserLevelRepository(DiscordRest discordRest, LevelsDatabase database) : base(discordRest) =>
        _database = database;

    public async Task<GuildUserLevel> GetOrCreateLevel(IGuildUser guildUser) =>
        await GetOrCreateLevel(guildUser?.GuildId ?? 0, guildUser?.Id ?? 0);

    public async Task<GuildUserLevel> GetOrCreateLevel(ulong guildid, ulong userid)
    {
        var level = _database.GetGuildUserLevel(guildid, userid);
        if (level is null)
        {
            level = new GuildUserLevel(guildid, userid);
            await _database.RegisterGuildUserLevel(level);
        }

        return level;
    }

    public GuildUserLevel GetLevel(ulong guildid, ulong userid) => _database.GetGuildUserLevel(guildid, userid);

    public async Task UpdateLevel(GuildUserLevel guildUserLevel)
    {
        if (_database.GetGuildUserLevel(guildUserLevel.GuildId, guildUserLevel.UserId) is not null)
            await _database.SaveChangesAsync();
        else
            await _database.UpdateGuildUserLevel(guildUserLevel);
    }

    public GuildUserLevel[] GetAllLevelsInGuild(ulong guildid) => _database.GetGuildUserLevelByGuild(guildid);

    public async Task Save() => await _database.SaveChangesAsync();
}
