using Bot.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MOTDs.Models;

namespace MOTDs.Data;

public class MotdDatabase : DataContext<MotdDatabase>, DataContextCreate
{
	public MotdDatabase(DbContextOptions<MotdDatabase> options) : base(options)
	{
	}

	private DbSet<GuildMotd> GuildMotDs { get; set; }

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