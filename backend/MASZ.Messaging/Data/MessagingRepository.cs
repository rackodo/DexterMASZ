using MASZ.Bot.Abstractions;
using MASZ.Bot.Dynamics;
using MASZ.Bot.Exceptions;
using MASZ.Bot.Services;
using MASZ.Messaging.Enums;
using MASZ.Messaging.Models;

namespace MASZ.Messaging.Data;

public class MessagingRepository : Repository, DeleteGuildData
{
	private readonly MessagingDatabase _messagingDatabase;

	public MessagingRepository(MessagingDatabase messagingDatabase, DiscordRest discordRest) : base(discordRest)
	{
		_messagingDatabase = messagingDatabase;
	}

	public async Task<List<ScheduledMessage>> GetDueMessages()
	{
		return await _messagingDatabase.GetDueMessages();
	}

	public async Task<List<ScheduledMessage>> GetAllMessages(ulong guildId, int page = 0)
	{
		return await _messagingDatabase.GetScheduledMessages(guildId, page);
	}

	public async Task<ScheduledMessage> GetMessage(int id)
	{
		var message = await _messagingDatabase.GetMessage(id);

		if (message == null)
			throw new ResourceNotFoundException($"ScheduledMessage with id {id} not found.");

		return message;
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

	public async Task DeleteMessagesForGuild(ulong guildId)
	{
		await _messagingDatabase.DeleteMessagesForGuild(guildId);
	}

	public async Task DeleteGuildData(ulong guildId)
	{
		await DeleteMessagesForGuild(guildId);
	}
}