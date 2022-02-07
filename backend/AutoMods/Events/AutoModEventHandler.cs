using AutoMods.Models;
using Bot.Abstractions;
using Bot.Models;
using Discord;

namespace AutoMods.Events;

public class AutoModEventHandler : InternalEventHandler
{
	internal readonly AsyncEvent<Func<AutoModConfig, IUser, Task>> AutoModConfigCreatedEvent = new();

	internal readonly AsyncEvent<Func<AutoModConfig, IUser, Task>> AutoModConfigDeletedEvent = new();

	internal readonly AsyncEvent<Func<AutoModConfig, IUser, Task>> AutoModConfigUpdatedEvent = new();

	internal readonly AsyncEvent<Func<AutoModEvent, AutoModConfig, GuildConfig, ITextChannel, IUser, Task>>
		AutoModEventRegisteredEvent = new();

	public event Func<AutoModConfig, IUser, Task> OnAutoModConfigCreated
	{
		add => AutoModConfigCreatedEvent.Add(value);
		remove => AutoModConfigCreatedEvent.Remove(value);
	}

	public event Func<AutoModConfig, IUser, Task> OnAutoModConfigUpdated
	{
		add => AutoModConfigUpdatedEvent.Add(value);
		remove => AutoModConfigUpdatedEvent.Remove(value);
	}

	public event Func<AutoModConfig, IUser, Task> OnAutoModConfigDeleted
	{
		add => AutoModConfigDeletedEvent.Add(value);
		remove => AutoModConfigDeletedEvent.Remove(value);
	}

	public event Func<AutoModEvent, AutoModConfig, GuildConfig, ITextChannel, IUser, Task> OnAutoModEventRegistered
	{
		add => AutoModEventRegisteredEvent.Add(value);
		remove => AutoModEventRegisteredEvent.Remove(value);
	}
}