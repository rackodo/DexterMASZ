using Discord;
using MASZ.Bot.Abstractions;
using MASZ.MOTDs.Models;

namespace MASZ.MOTDs.Events;

public class MotdEventHandler : Bot.Abstractions.EventHandler
{
	internal readonly AsyncEvent<Func<GuildMotd, IUser, Task>> GuildMotdCreatedEvent = new();

	internal readonly AsyncEvent<Func<GuildMotd, IUser, Task>> GuildMotdUpdatedEvent = new();

	public event Func<GuildMotd, IUser, Task> OnGuildMotdCreated
	{
		add => GuildMotdCreatedEvent.Add(value);
		remove => GuildMotdCreatedEvent.Remove(value);
	}

	public event Func<GuildMotd, IUser, Task> OnGuildMotdUpdated
	{
		add => GuildMotdUpdatedEvent.Add(value);
		remove => GuildMotdUpdatedEvent.Remove(value);
	}
}