using MASZ.Bot.Abstractions;
using MASZ.Bot.Dynamics;
using MASZ.MOTDs.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MASZ.MOTDs.Data;

public class MotdDatabase : DataContext<MotdDatabase>, DataContextCreate, DeleteGuildData
{
	public MotdDatabase(DbContextOptions<MotdDatabase> options) : base(options)
	{
	}

	private DbSet<GuildMotd> GuildMotDs { get; set; }

	public static void AddContextToServiceProvider(Action<DbContextOptionsBuilder> optionsAction,
		IServiceCollection serviceCollection)
	{
		serviceCollection.AddDbContext<MotdDatabase>(optionsAction);
	}

	public async Task DeleteGuildData(ulong guildId)
	{
		await DeleteMotdForGuild(guildId);
	}

	public async Task<GuildMotd> GetMotdForGuild(ulong guildId)
	{
		return await GuildMotDs.AsQueryable().Where(x => x.GuildId == guildId).FirstOrDefaultAsync();
	}

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