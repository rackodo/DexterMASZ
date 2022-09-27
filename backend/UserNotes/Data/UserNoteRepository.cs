using Bot.Abstractions;
using Bot.Dynamics;
using Bot.Enums;
using Bot.Exceptions;
using Bot.Extensions;
using Bot.Services;
using Discord;
using Humanizer;
using UserNotes.Events;
using UserNotes.Models;
using UserNotes.Translators;
using Utilities.Dynamics;

namespace UserNotes.Data;

public class UserNoteRepository : Repository,
	AddAdminStats, AddGuildStats, AddSearch, AddNetworks, WhoIsResults, DeleteGuildData
{
	private readonly DiscordRest _discordRest;
	private readonly UserNoteEventHandler _eventHandler;
	private readonly UserNoteDatabase _userNoteDatabase;

	public UserNoteRepository(DiscordRest discordRest, UserNoteDatabase userNoteDatabase,
		UserNoteEventHandler eventHandler) : base(discordRest)
	{
		_userNoteDatabase = userNoteDatabase;
		_discordRest = discordRest;
		_eventHandler = eventHandler;
	}

	public async Task DeleteGuildData(ulong guildId)
	{
		await _userNoteDatabase.DeleteUserNoteByGuild(guildId);
	}

	public async Task AddAdminStatistics(dynamic adminStats)
	{
		adminStats.userNotes = await CountUserNotes();
	}

	public async Task AddGuildStatistics(dynamic stats, ulong guildId)
	{
		stats.userNotes = await CountUserNotesForGuild(guildId);
	}

	public async Task AddNetworkData(dynamic network, List<string> modGuilds, ulong userId)
	{
		network.userNotes = (await GetUserNotesByUser(userId)).Where(x => modGuilds.Contains(x.GuildId.ToString())).ToList();
	}

	public async Task AddSearchData(dynamic data, ulong guildId, string search)
	{
		UserNoteExpanded userNote = null;
		try
		{
			var userId = ulong.Parse(search);
			var note = await GetUserNote(guildId, userId);
			userNote = new UserNoteExpanded(
				note,
				await _discordRest.FetchUserInfo(note.UserId),
				await _discordRest.FetchUserInfo(note.CreatorId)
			);
		}
		catch (ResourceNotFoundException)
		{
		}
		catch (FormatException)
		{
		}
		catch (ArgumentException)
		{
		}
		catch (OverflowException)
		{
		}

		data.userNoteView = userNote;
	}

	public async Task AddWhoIsInformation(EmbedBuilder embed, IGuildUser user, IInteractionContext context,
		Translation translator)
	{
		try
		{
			var userNote = await GetUserNote(context.Guild.Id, user.Id);
			embed.AddField(translator.Get<UserNoteTranslator>().UserNote(), userNote.Description.Truncate(1000));
		}
		catch (ResourceNotFoundException)
		{
		}
	}

	public async Task<UserNote> GetUserNote(ulong guildId, ulong userId)
	{
		var userNote = await _userNoteDatabase.GetUserNoteByUserIdAndGuildId(userId, guildId);

		if (userNote == null)
			throw new ResourceNotFoundException($"UserNote for guild {guildId} and user {userId} not found.");

		return userNote;
	}

	public async Task<List<UserNote>> GetUserNotesByGuild(ulong guildId)
	{
		return await _userNoteDatabase.GetUserNotesByGuildId(guildId);
	}

	public async Task<List<UserNote>> GetUserNotesByUser(ulong userId)
	{
		return await _userNoteDatabase.GetUserNotesByUserId(userId);
	}

	public async Task<UserNote> CreateOrUpdateUserNote(ulong guildId, ulong userId, string content)
	{
		var validUser = await _discordRest.FetchUserInfo(userId);

		if (validUser == null)
			throw new InvalidIUserException("User not found", userId);

		UserNote userNote;
		var action = RestAction.Updated;

		try
		{
			userNote = await GetUserNote(guildId, userId);
		}
		catch (ResourceNotFoundException)
		{
			userNote = new UserNote
			{
				GuildId = guildId,
				UserId = userId
			};
			action = RestAction.Created;
		}

		userNote.UpdatedAt = DateTime.UtcNow;
		userNote.CreatorId = Identity.Id;

		userNote.Description = content;

		await _userNoteDatabase.SaveUserNote(userNote);

		if (action == RestAction.Created)
			_eventHandler.UserNoteCreatedEvent.Invoke(userNote, Identity);
		else
			_eventHandler.UserNoteUpdatedEvent.Invoke(userNote, Identity);

		return userNote;
	}

	public async Task DeleteUserNote(ulong guildId, ulong userId)
	{
		var userNote = await GetUserNote(guildId, userId);

		await _userNoteDatabase.DeleteUserNote(userNote);

		_eventHandler.UserNoteDeletedEvent.Invoke(userNote, Identity);
	}

	public async Task<int> CountUserNotesForGuild(ulong guildId)
	{
		return await _userNoteDatabase.CountUserNotesForGuild(guildId);
	}

	public async Task<int> CountUserNotes()
	{
		return await _userNoteDatabase.CountUserNotes();
	}
}