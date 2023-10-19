using Bot.Abstractions;
using Bot.DTOs;
using Bot.Services;
using Discord;
using Microsoft.EntityFrameworkCore;

namespace Bot.Data;

public class UserRepository(DiscordRest discordRest, BotDatabase context) : Repository(discordRest)
{
    private readonly BotDatabase _context = context;

    public async Task<LeftUserDto> TryGetUser(ulong userId)
    {
        var user = await _context.GetLeftUser(userId);

        if (user == null)
            return null;

        _context.Entry(user).State = EntityState.Detached;

        return new LeftUserDto(user);
    }

    public async Task UpdateUser(IUser user)
        => await _context.UpdateUser(new LeftUserDto(user).CreateUserFromDto());

    public async Task AddUser(IUser user)
    {
        await RemoveUserIfExists(user);
        await _context.AddLeftUser(new LeftUserDto(user).CreateUserFromDto());
    }

    public async Task RemoveUserIfExists(IUser user)
    {
        var userLeft = await _context.GetLeftUser(user.Id);
        if (userLeft != null)
            await _context.RemoveLeftUser(userLeft);
    }
}
