using Bot.Abstractions;
using Bot.Services;
using PrivateVcs.Models;

namespace PrivateVcs.Data;

public class PrivateVcConfigRepository : Repository
{
    private readonly PrivateVcDatabase _privateVcDatabase;

    public PrivateVcConfigRepository(PrivateVcDatabase privateVcDatabase,
        DiscordRest discordRest) : base(discordRest) =>
        _privateVcDatabase = privateVcDatabase;

    public async Task<PrivateVcConfig> SelectPrivateVcConfig(ulong guildId) =>
        await _privateVcDatabase.PrivateVcConfigs.FindAsync(guildId);

    public async Task<PrivateVcConfig> PutPrivateVcConfig(PrivateVcConfig newPrivateVcConfig) =>
        await _privateVcDatabase.PutPrivateVcConfig(newPrivateVcConfig);
}
