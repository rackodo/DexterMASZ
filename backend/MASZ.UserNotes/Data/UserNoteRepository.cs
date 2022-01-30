using Discord;
using MASZ.Bot.Abstractions;
using MASZ.Bot.Dynamics;
using MASZ.Bot.Enums;
using MASZ.Bot.Exceptions;
using MASZ.Bot.Extensions;
using MASZ.Bot.Services;
using MASZ.UserNotes.Events;
using MASZ.UserNotes.Models;
using MASZ.UserNotes.Translators;
using MASZ.UserNotes.Views;
using MASZ.Utilities.Dynamics;

namespace MASZ.UserNotes.Data;

public class UserNoteRepository : Repository,
	AddAdminStats, CacheUsers, AddGuildStats, AddSearch, AddNetworks, WhoIsResults, DeleteGuildData
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
		network.userNotes = (await GetUserNotesByUser(userId)).Where(x => modGuilds.Contains(x.GuildId.ToString()))
			.Select(x => new UserNoteView(x)).ToList();
	}

	public async Task AddSearchData(dynamic data, ulong guildId, string search)
	{
		UserNoteExpandedView userNote = null;
		try
		{
			var userId = ulong.Parse(search);
			var note = await GetUserNote(guildId, userId);
			userNote = new UserNoteExpandedView(
				note,
				await _discordRest.FetchUserInfo(note.UserId, CacheBehavior.OnlyCache),
				await _discordRest.FetchUserInfo(note.CreatorId, CacheBehavior.OnlyCache)
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

	public async Task CacheKnownUsers(List<ulong> handledUsers)
	{
		foreach (var userNote in await _userNoteDatabase.SelectLatestUserNotes(DateTime.UtcNow.AddYears(-3), 100))
		{
			if (!handledUsers.Contains(userNote.UserId))
			{
				await _discordRest.FetchUserInfo(userNote.UserId, CacheBehavior.IgnoreCache);
				handledUsers.Add(userNote.UserId);
			}

			if (handledUsers.Contains(userNote.CreatorId)) continue;

			await _discordRest.FetchUserInfo(userNote.CreatorId, CacheBehavior.IgnoreCache);
			handledUsers.Add(userNote.CreatorId);
		}
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
		var validUser = await _discordRest.FetchUserInfo(userId, CacheBehavior.Default);

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