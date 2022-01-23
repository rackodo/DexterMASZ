using MASZ.Bot.Abstractions;
using MASZ.Bot.Dynamics;
using MASZ.UserMaps.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MASZ.UserMaps.Data;

public class UserMapDatabase : DataContext<UserMapDatabase>, DataContextCreate, DeleteGuildData
{
	public UserMapDatabase(DbContextOptions<UserMapDatabase> options) : base(options)
	{
	}

	public DbSet<UserMap> UserMaps { get; set; }

	public static void AddContextToServiceProvider(Action<DbContextOptionsBuilder> optionsAction,
		IServiceCollection serviceCollection)
	{
		serviceCollection.AddDbContext<UserMapDatabase>(optionsAction);
	}

	public async Task DeleteGuildData(ulong guildId)
	{
		await DeleteUserMapByGuild(guildId);
	}

	public async Task<List<UserMap>> SelectLatestUserMaps(DateTime timeLimit, int limit)
	{
		return await UserMaps.AsQueryable().Where(x => x.CreatedAt > timeLimit).OrderByDescending(x => x.CreatedAt)
			.Take(limit).ToListAsync();
	}

	public async Task<UserMap> GetUserMapById(int id)
	{
		return await UserMaps.AsQueryable().Where(x => x.Id == id).FirstOrDefaultAsync();
	}

	public async Task<List<UserMap>> GetUserMapsByUserId(ulong userId)
	{
		return await UserMaps.AsQueryable().Where(x => x.UserA == userId || x.UserB == userId)
			.OrderByDescending(x => x.CreatedAt).ToListAsync();
	}

	public async Task<List<UserMap>> GetUserMapsByUserIdAndGuildId(ulong userId, ulong guildId)
	{
		return await UserMaps.AsQueryable()
			.Where(x => (x.UserA == userId || x.UserB == userId) && x.GuildId == guildId)
			.OrderByDescending(x => x.CreatedAt).ToListAsync();
	}

	public async Task<UserMap> GetUserMapByUserIdsAndGuildId(ulong userAId, ulong userBId, ulong guildId)
	{
		return await UserMaps.AsQueryable().Where(x =>
			(x.UserA == userAId || x.UserB == userAId) && (x.UserA == userBId || x.UserB == userBId) &&
			x.GuildId == guildId).FirstOrDefaultAsync();
	}

	public async Task<List<UserMap>> GetUserMapsByGuildId(ulong guildId)
	{
		return await UserMaps.AsQueryable().Where(x => x.GuildId == guildId).OrderByDescending(x => x.CreatedAt)
			.ToListAsync();
	}

	public async Task<int> CountUserMaps()
	{
		return await UserMaps.AsQueryable().CountAsync();
	}

	public async Task<int> CountUserMapsForGuild(ulong guildId)
	{
		return await UserMaps.AsQueryable().Where(x => x.GuildId == guildId).CountAsync();
	}

	public async Task DeleteUserMap(UserMap userMaps)
	{
		UserMaps.Remove(userMaps);
		await SaveChangesAsync();
	}

	public async Task SaveUserMap(UserMap userMaps)
	{
		UserMaps.Update(userMaps);
		await SaveChangesAsync();
	}

	public async Task DeleteUserMapByGuild(ulong guildId)
	{
		var userMaps = await UserMaps.AsQueryable().Where(x => x.GuildId == guildId).ToListAsync();
		UserMaps.RemoveRange(userMaps);
		await SaveChangesAsync();
	}
}