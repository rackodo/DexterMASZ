using MASZ.Bot.Abstractions;
using MASZ.Bot.Dynamics;
using MASZ.GuildAudits.Enums;
using MASZ.GuildAudits.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MASZ.GuildAudits.Data;

public class GuildAuditDatabase : DataContext<GuildAuditDatabase>, DataContextCreate
{
	public GuildAuditDatabase(DbContextOptions<GuildAuditDatabase> options) : base(options)
	{
	}

	public DbSet<GuildAuditConfig> GuildAuditConfigs { get; set; }

	public static void AddContextToServiceProvider(Action<DbContextOptionsBuilder> optionsAction,
		IServiceCollection serviceCollection)
	{
		serviceCollection.AddDbContext<GuildAuditDatabase>(optionsAction);
	}

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
			.FirstOrDefaultAsync(x => x.GuildId == guildId && x.GuildAuditLogEvent == type);
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