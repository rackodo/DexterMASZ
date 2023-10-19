using Bot.Abstractions;
using GuildAudits.Enums;
using GuildAudits.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GuildAudits.Data;

public class GuildAuditDatabase(DbContextOptions<GuildAuditDatabase> options) : DataContext<GuildAuditDatabase>(options), IDataContextCreate
{
    public DbSet<GuildAuditConfig> GuildAuditConfigs { get; set; }

    public static void AddContextToServiceProvider(Action<DbContextOptionsBuilder> optionsAction,
        IServiceCollection serviceCollection) =>
        serviceCollection.AddDbContext<GuildAuditDatabase>(optionsAction);

    public async Task DeleteAllAuditLogConfigsForGuild(ulong guildId)
    {
        var events = await GuildAuditConfigs.AsQueryable().Where(x => x.GuildId == guildId).ToListAsync();
        GuildAuditConfigs.RemoveRange(events);
    }

    public async Task<List<GuildAuditConfig>> SelectAllAuditLogConfigsForGuild(ulong guildId) =>
        await GuildAuditConfigs.AsQueryable().Where(x => x.GuildId == guildId).ToListAsync();

    public async Task<GuildAuditConfig> SelectAuditLogConfigForGuildAndType(ulong guildId, GuildAuditLogEvent type) =>
        await GuildAuditConfigs.AsQueryable()
            .FirstOrDefaultAsync(x => x.GuildId == guildId && x.GuildAuditLogEvent == type);

    public async Task PutAuditLogConfig(GuildAuditConfig auditLogConfig)
    {
        GuildAuditConfigs.Update(auditLogConfig);
        await SaveChangesAsync();
    }

    public async Task DeleteSpecificAuditLogConfig(GuildAuditConfig auditLogConfig)
    {
        GuildAuditConfigs.Remove(auditLogConfig);
        await SaveChangesAsync();
    }
}
