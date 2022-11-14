using Bot.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MOTDs.Models;

namespace MOTDs.Data;

public class MotdDatabase : DataContext<MotdDatabase>, DataContextCreate
{
    private DbSet<GuildMotd> GuildMotDs { get; set; }

    public MotdDatabase(DbContextOptions<MotdDatabase> options) : base(options)
    {
    }

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