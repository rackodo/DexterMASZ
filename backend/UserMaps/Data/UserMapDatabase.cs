using Bot.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UserMaps.Models;

namespace UserMaps.Data;

public class UserMapDatabase : DataContext<UserMapDatabase>, DataContextCreate
{
    public DbSet<UserMap> UserMaps { get; set; }

    public UserMapDatabase(DbContextOptions<UserMapDatabase> options) : base(options)
    {
    }

    public static void AddContextToServiceProvider(Action<DbContextOptionsBuilder> optionsAction,
        IServiceCollection serviceCollection) =>
        serviceCollection.AddDbContext<UserMapDatabase>(optionsAction);

    public async Task<List<UserMap>> SelectLatestUserMaps(DateTime timeLimit, int limit) =>
        await UserMaps.AsQueryable().Where(x => x.CreatedAt > timeLimit).OrderByDescending(x => x.CreatedAt)
            .Take(limit).ToListAsync();

    public async Task<UserMap> GetUserMapById(int id) =>
        await UserMaps.AsQueryable().Where(x => x.Id == id).FirstOrDefaultAsync();

    public async Task<List<UserMap>> GetUserMapsByUserId(ulong userId) =>
        await UserMaps.AsQueryable().Where(x => x.UserA == userId || x.UserB == userId)
            .OrderByDescending(x => x.CreatedAt).ToListAsync();

    public async Task<List<UserMap>> GetUserMapsByUserIdAndGuildId(ulong userId, ulong guildId) =>
        await UserMaps.AsQueryable()
            .Where(x => (x.UserA == userId || x.UserB == userId) && x.GuildId == guildId)
            .OrderByDescending(x => x.CreatedAt).ToListAsync();

    public async Task<UserMap> GetUserMapByUserIdsAndGuildId(ulong userAId, ulong userBId, ulong guildId) =>
        await UserMaps.AsQueryable().Where(x =>
            (x.UserA == userAId || x.UserB == userAId) && (x.UserA == userBId || x.UserB == userBId) &&
            x.GuildId == guildId).FirstOrDefaultAsync();

    public async Task<List<UserMap>> GetUserMapsByGuildId(ulong guildId) =>
        await UserMaps.AsQueryable().Where(x => x.GuildId == guildId).OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

    public async Task<int> CountUserMaps() => await UserMaps.AsQueryable().CountAsync();

    public async Task<int> CountUserMapsForGuild(ulong guildId) =>
        await UserMaps.AsQueryable().Where(x => x.GuildId == guildId).CountAsync();

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