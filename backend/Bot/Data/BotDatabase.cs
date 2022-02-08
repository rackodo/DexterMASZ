using Bot.Abstractions;
using Bot.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Bot.Data;

public class BotDatabase : DataContext<BotDatabase>, DataContextCreate
{
	public BotDatabase(DbContextOptions<BotDatabase> options) : base(options)
	{
	}

	private DbSet<AppSettings> AppSettings { get; set; }

	public DbSet<GuildConfig> GuildConfigs { get; set; }

	public static void AddContextToServiceProvider(Action<DbContextOptionsBuilder> optionsAction,
		IServiceCollection serviceCollection)
	{
		serviceCollection.AddDbContext<BotDatabase>(optionsAction);
	}

	public async Task<AppSettings> GetAppSettings(ulong clientId)
	{
		return await AppSettings.FindAsync(clientId);
	}

	public async Task UpdateAppSetting(AppSettings appSettings)
	{
		AppSettings.Update(appSettings);
		await SaveChangesAsync();
	}

	public async Task AddAppSetting(AppSettings appSettings)
	{
		AppSettings.Add(appSettings);
		await SaveChangesAsync();
	}

	public async Task<GuildConfig> SelectSpecificGuildConfig(ulong guildId)
	{
		return await GuildConfigs.AsQueryable().FirstOrDefaultAsync(x => x.GuildId == guildId);
	}

	public async Task<List<GuildConfig>> SelectAllGuildConfigs()
	{
		return await GuildConfigs.AsQueryable().ToListAsync();
	}

	public async Task DeleteSpecificGuildConfig(GuildConfig guildConfig)
	{
		GuildConfigs.Remove(guildConfig);
		await SaveChangesAsync();
	}

	public async Task InternalUpdateGuildConfig(GuildConfig guildConfig)
	{
		GuildConfigs.Update(guildConfig);
		await SaveChangesAsync();
	}

	public async Task SaveGuildConfig(GuildConfig guildConfig)
	{
		await GuildConfigs.AddAsync(guildConfig);
		await SaveChangesAsync();
	}

	public async Task<int> CountAllGuildConfigs()
	{
		return await GuildConfigs.AsQueryable().CountAsync();
	}
}