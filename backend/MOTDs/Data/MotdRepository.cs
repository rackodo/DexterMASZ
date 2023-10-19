using Bot.Abstractions;
using Bot.Dynamics;
using Bot.Enums;
using Bot.Exceptions;
using Bot.Extensions;
using Bot.Services;
using MOTDs.Events;
using MOTDs.Models;

namespace MOTDs.Data;

public class MotdRepository(DiscordRest discordRest, MotdDatabase guildMotdDatabase,
    MotdEventHandler eventHandler) : Repository(discordRest), IDeleteGuildData
{
    private readonly MotdEventHandler _eventHandler = eventHandler;
    private readonly MotdDatabase _guildMotdDatabase = guildMotdDatabase;

    public async Task DeleteGuildData(ulong guildId) => await _guildMotdDatabase.DeleteMotdForGuild(guildId);

    public async Task<GuildMotd> GetMotd(ulong guildId)
    {
        var motd = await _guildMotdDatabase.GetMotdForGuild(guildId);

        return motd ?? throw new ResourceNotFoundException();
    }

    public async Task<GuildMotd> CreateOrUpdateMotd(ulong guildId, string content, bool visible)
    {
        var action = RestAction.Updated;
        GuildMotd motd;

        try
        {
            motd = await GetMotd(guildId);
        }
        catch (ResourceNotFoundException)
        {
            motd = new GuildMotd
            {
                GuildId = guildId
            };
            action = RestAction.Created;
        }

        motd.CreatedAt = DateTime.UtcNow;
        motd.UserId = Identity.Id;
        motd.Message = content;
        motd.ShowMotd = visible;

        await _guildMotdDatabase.SaveMotd(motd);

        if (action == RestAction.Created)
            _eventHandler.GuildMotdCreatedEvent.Invoke(motd, Identity);
        else
            _eventHandler.GuildMotdUpdatedEvent.Invoke(motd, Identity);

        return motd;
    }
}
