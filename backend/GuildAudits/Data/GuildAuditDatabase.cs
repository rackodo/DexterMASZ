using Bot.Abstractions;
using GuildAudits.Enums;
using GuildAudits.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GuildAudits.Data;

public class GuildAuditDatabase : DataContext<GuildAuditDatabase>, DataContextCreate
{
	public static void AddContextToServiceProvider(Action<DbContextOptionsBuilder> optionsAction,
		IServiceCollection serviceCollection)
	{
		serviceCollection.AddDbContext<GuildAuditDatabase>(optionsAction);
	}

	public GuildAuditDatabase(DbContextOptions<GuildAuditDatabase> options) : base(options)
	{
	}

	public DbSet<GuildAuditConfig> GuildAuditConfigs { get; set; }

	public async Task DeleteAllAuditLogConfigsForGuild(ulong guildId)
	{
		var events = await GuildAuditConfigs.AsQueryable().Where(x => x.GuildId == guildId).ToListAsync();
		GuildAuditConfigs.RemoveRange(events);
	}

	public async Task<List<GuildAuditConfig>> SelectAllAuditLogConfigsForGuild(ulong guildId)
	{
		return await GuildAuditConfigs.AsQueryable().Where(x => x.GuildId == guildId).ToListAsync();
	}

	public async Task<GuildAuditConfig> SelectAuditLogConfigForGuildAndType(ulong guildId, GuildAuditEvent type)
	{
		return await GuildAuditConfigs.AsQueryable()
			.FirstOrDefaultAsync(x => x.GuildId == guildId && x.GuildAuditEvent == type);
	}

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