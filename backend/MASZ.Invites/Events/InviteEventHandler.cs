using Discord.WebSocket;
using MASZ.Bot.Abstractions;
using MASZ.Invites.Models;

namespace MASZ.Invites.Events;

public class InviteEventHandler : InternalEventHandler
{
	internal readonly AsyncEvent<Func<SocketGuildChannel, TrackedInvite, Task>> InviteDeletedEvent = new();

	internal readonly AsyncEvent<Func<UserInvite, Task>> InviteUsageRegisteredEvent = new();

	public event Func<UserInvite, Task> OnInviteUsageRegistered
	{
		add => InviteUsageRegisteredEvent.Add(value);
		remove => InviteUsageRegisteredEvent.Remove(value);
	}

	public event Func<SocketGuildChannel, TrackedInvite, Task> OnInviteDeleted
	{
		add => InviteDeletedEvent.Add(value);
		remove => InviteDeletedEvent.Remove(value);
	}
}