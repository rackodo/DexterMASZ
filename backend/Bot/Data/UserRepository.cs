using Bot.Abstractions;
using Bot.DTOs;
using Bot.Services;
using Discord;
using Microsoft.EntityFrameworkCore;

namespace Bot.Data;

public class UserRepository : Repository
{
	private readonly BotDatabase _context;

	public UserRepository(DiscordRest discordRest, BotDatabase context) : base(discordRest)
		=> _context = context;

	public async Task<LeftUserDTO> TryGetUser(ulong userId)
	{
		var user = await _context.GetLeftUser(userId);

		_context.Entry(user).State = EntityState.Detached;

		if (user == null)
			return null;

		return new LeftUserDTO(user);
	}

	public async Task UpdateUser(IUser user)
		=> await _context.UpdateUser(new LeftUserDTO(user).CreateUserFromDto());

	public async Task AddUser(IUser user)
	{
		await RemoveUserIfExists(user);
		await _context.AddLeftUser(new LeftUserDTO(user).CreateUserFromDto());
	}

	public async Task RemoveUserIfExists(IUser user)
	{
		var userLeft = await _context.GetLeftUser(user.Id);
		if (userLeft != null)
			await _context.RemoveLeftUser(userLeft);
	}
}
