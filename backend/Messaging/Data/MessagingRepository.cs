using Bot.Abstractions;
using Bot.Dynamics;
using Bot.Exceptions;
using Bot.Services;
using Messaging.Enums;
using Messaging.Models;

namespace Messaging.Data;

public class MessagingRepository(MessagingDatabase messagingDatabase, DiscordRest discordRest) : Repository(discordRest), IDeleteGuildData
{
    private readonly MessagingDatabase _messagingDatabase = messagingDatabase;

    public async Task DeleteGuildData(ulong guildId) => await DeleteMessagesForGuild(guildId);

    public async Task<List<ScheduledMessage>> GetDueMessages() => await _messagingDatabase.GetDueMessages();

    public async Task<List<ScheduledMessage>> GetAllMessages(ulong guildId, int page = 0) =>
        await _messagingDatabase.GetScheduledMessages(guildId, page);

    public async Task<ScheduledMessage> GetMessage(int id)
    {
        var message = await _messagingDatabase.GetMessage(id);

        return message ?? throw new ResourceNotFoundException($"ScheduledMessage with id {id} not found.");
    }

    public async Task<ScheduledMessage> CreateMessage(ScheduledMessage message)
    {
        message.CreatedAt = DateTime.UtcNow;
        message.LastEditedAt = message.CreatedAt;
        message.CreatorId = Identity.Id;
        message.LastEditedById = Identity.Id;
        message.Status = ScheduledMessageStatus.Pending;

        await _messagingDatabase.SaveMessage(message);

        return message;
    }

    public async Task<ScheduledMessage> UpdateMessage(ScheduledMessage message)
    {
        message.LastEditedAt = DateTime.UtcNow;
        message.LastEditedById = Identity.Id;

        await _messagingDatabase.UpdateMessage(message);

        return message;
    }

    public async Task<ScheduledMessage> SetMessageAsSent(int id)
    {
        var message = await GetMessage(id);

        message.Status = ScheduledMessageStatus.Sent;

        await _messagingDatabase.UpdateMessage(message);

        return message;
    }

    public async Task<ScheduledMessage> SetMessageAsFailed(int id, ScheduledMessageFailureReason reason)
    {
        var message = await GetMessage(id);

        message.Status = ScheduledMessageStatus.Failed;
        message.FailureReason = reason;

        await _messagingDatabase.UpdateMessage(message);

        return message;
    }

    public async Task<ScheduledMessage> DeleteMessage(int id)
    {
        var message = await GetMessage(id);

        await _messagingDatabase.DeleteMessage(message);

        return message;
    }

    public async Task DeleteMessagesForGuild(ulong guildId) => await _messagingDatabase.DeleteMessagesForGuild(guildId);
}
