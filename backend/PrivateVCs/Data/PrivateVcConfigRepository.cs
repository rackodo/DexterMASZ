using Bot.Abstractions;
using Bot.Services;
using PrivateVcs.Models;

namespace PrivateVcs.Data;

public class PrivateVcConfigRepository(PrivateVcDatabase privateVcDatabase,
    DiscordRest discordRest) : Repository(discordRest)
{
    private readonly PrivateVcDatabase _privateVcDatabase = privateVcDatabase;

    public async Task<PrivateVcConfig> SelectPrivateVcConfig(ulong guildId) =>
        await _privateVcDatabase.PrivateVcConfigs.FindAsync(guildId);

    public async Task<PrivateVcConfig> PutPrivateVcConfig(PrivateVcConfig newPrivateVcConfig) =>
        await _privateVcDatabase.PutPrivateVcConfig(newPrivateVcConfig);
}
