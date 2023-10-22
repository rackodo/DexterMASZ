using Bot.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MOTDs.Models;

namespace MOTDs.Data;

public class MotdDatabase(DbContextOptions<MotdDatabase> options) : DataContext<MotdDatabase>(options), IDataContextCreate
{
    private DbSet<GuildMotd> GuildMotDs { get; set; }

    public static void AddContextToServiceProvider(Action<DbContextOptionsBuilder> optionsAction,
        IServiceCollection serviceCollection) =>
        serviceCollection.AddDbContext<MotdDatabase>(optionsAction);

    public async Task<GuildMotd> GetMotdForGuild(ulong guildId) =>
        await GuildMotDs.AsQueryable().Where(x => x.GuildId == guildId).FirstOrDefaultAsync();

    public async Task SaveMotd(GuildMotd motd)
    {
        GuildMotDs.Update(motd);
        await SaveChangesAsync();
    }

    public async Task DeleteMotdForGuild(ulong guildId)
    {
        var motd = await GuildMotDs.AsQueryable().Where(x => x.GuildId == guildId).ToListAsync();
        GuildMotDs.RemoveRange(motd);
        await SaveChangesAsync();
    }
}
