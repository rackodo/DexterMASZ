using MASZ.Bot.Abstractions;
using MASZ.Bot.Models;

namespace MASZ.Bot.Events;

public class BotEventHandler : InternalEventHandler
{
	public readonly AsyncEvent<Func<Task>> BotLaunchedEvent = new();

	public readonly AsyncEvent<Func<GuildConfig, Task>> GuildDeletedEvent = new();

	public readonly AsyncEvent<Func<GuildConfig, bool, Task>> GuildRegisteredEvent = new();

	public readonly AsyncEvent<Func<GuildConfig, Task>> GuildUpdatedEvent = new();

	public readonly AsyncEvent<Func<Identity, Task>> IdentityRegisteredEvent = new();

	public readonly AsyncEvent<Func<Identity, Task>> IdentityRemovedEvent = new();

	public readonly AsyncEvent<Func<int, DateTime, Task>> InternalCachingDoneEvent = new();

	internal readonly AsyncEvent<Func<ApiToken, Task>> TokenCreatedEvent = new();

	internal readonly AsyncEvent<Func<ApiToken, Task>> TokenDeletedEvent = new();

	public event Func<Task> OnBotLaunched
	{
		add => BotLaunchedEvent.Add(value);
		remove => BotLaunchedEvent.Remove(value);
	}

	public event Func<Identity, Task> OnIdentityRegistered
	{
		add => IdentityRegisteredEvent.Add(value);
		remove => IdentityRegisteredEvent.Remove(value);
	}

	public event Func<Identity, Task> OnIdentityRemoved
	{
		add => IdentityRemovedEvent.Add(value);
		remove => IdentityRemovedEvent.Remove(value);
	}

	public event Func<GuildConfig, bool, Task> OnGuildRegistered
	{
		add => GuildRegisteredEvent.Add(value);
		remove => GuildRegisteredEvent.Remove(value);
	}

	public event Func<GuildConfig, Task> OnGuildUpdated
	{
		add => GuildUpdatedEvent.Add(value);
		remove => GuildUpdatedEvent.Remove(value);
	}

	public event Func<GuildConfig, Task> OnGuildDeleted
	{
		add => GuildDeletedEvent.Add(value);
		remove => GuildDeletedEvent.Remove(value);
	}

	public event Func<int, DateTime, Task> OnInternalCachingDone
	{
		add => InternalCachingDoneEvent.Add(value);
		remove => InternalCachingDoneEvent.Remove(value);
	}

	public event Func<ApiToken, Task> OnTokenCreated
	{
		add => TokenCreatedEvent.Add(value);
		remove => TokenCreatedEvent.Remove(value);
	}

	public event Func<ApiToken, Task> OnTokenDeleted
	{
		add => TokenDeletedEvent.Add(value);
		remove => TokenDeletedEvent.Remove(value);
	}
}