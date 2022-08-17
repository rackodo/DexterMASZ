using Bot.Abstractions;
using Invites.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Invites.Data;

public class InviteDatabase : DataContext<InviteDatabase>, DataContextCreate
{
	public InviteDatabase(DbContextOptions<InviteDatabase> options) : base(options)
	{
	}

	public DbSet<UserInvite> UserInvites { get; set; }

	public async Task<List<UserInvite>> GetInvitedUsersByUser(ulong userId)
	{
		return await UserInvites.AsQueryable().Where(x => x.InviteIssuerId == userId).ToListAsync();
	}

	public async Task<List<UserInvite>> GetUsedInvitesByUser(ulong userId)
	{
		return await UserInvites.AsQueryable().Where(x => x.JoinedUserId == userId).ToListAsync();
	}

	public async Task<List<UserInvite>> GetUsedInvitesByUserAndGuild(ulong userId, ulong guildId)
	{
		return await UserInvites.AsQueryable().Where(x => x.JoinedUserId == userId && x.GuildId == guildId)
			.ToListAsync();
	}

	public async Task<int> CountTrackedInvites()
	{
		return await UserInvites.AsQueryable().CountAsync();
	}

	public async Task<int> CountTrackedInvitesForGuild(ulong guildId)
	{
		return await UserInvites.AsQueryable().Where(x => x.GuildId == guildId).CountAsync();
	}

	public async Task<List<UserInvite>> GetInvitesByCode(string code)
	{
		return await UserInvites.AsQueryable().Where(x => x.UsedInvite == code).ToListAsync();
	}

	public async Task DeleteInviteHistoryByGuild(ulong guildId)
	{
		var userInvites = await UserInvites.AsQueryable().Where(x => x.GuildId == guildId).ToListAsync();
		UserInvites.RemoveRange(userInvites);
		await SaveChangesAsync();
	}

	public async Task SaveInvite(UserInvite userInvite)
	{
		await UserInvites.AddAsync(userInvite);
		await SaveChangesAsync();
	}
}