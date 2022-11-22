using Bot.Abstractions;
using Bot.Services;
using Punishments.Models;

namespace Punishments.Data;

public class PunishmentConfigRepository : Repository
{
    private readonly PunishmentDatabase _punishmentDatabase;

    public PunishmentConfigRepository(PunishmentDatabase punishmentDatabase,
        DiscordRest discordRest) : base(discordRest) =>
        _punishmentDatabase = punishmentDatabase;

    public async Task<PunishmentConfig> GetGuildPunishmentConfig(ulong guildId) =>
        await _punishmentDatabase.SelectPunishmentConfig(guildId);
}
