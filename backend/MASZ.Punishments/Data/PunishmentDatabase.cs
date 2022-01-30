using MASZ.Bot.Abstractions;
using MASZ.Bot.Models;
using MASZ.Punishments.Enums;
using MASZ.Punishments.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MASZ.Punishments.Data;

public class PunishmentDatabase : DataContext<PunishmentDatabase>, DataContextCreate
{
	public PunishmentDatabase(DbContextOptions<PunishmentDatabase> options) : base(options)
	{
	}

	public DbSet<CaseTemplate> CaseTemplates { get; set; }

	public DbSet<ModCaseComment> ModCaseComments { get; set; }

	public DbSet<ModCase> ModCases { get; set; }

	public static void AddContextToServiceProvider(Action<DbContextOptionsBuilder> optionsAction,
		IServiceCollection serviceCollection)
	{
		serviceCollection.AddDbContext<PunishmentDatabase>(optionsAction);
	}

	public override void OverrideModelCreating(ModelBuilder builder)
	{
		builder.Entity<ModCaseComment>()
			.HasOne(c => c.ModCase)
			.WithMany(c => c.Comments)
			.IsRequired()
			.OnDelete(DeleteBehavior.Cascade);
	}

	public async Task<CaseTemplate> GetSpecificCaseTemplate(int templateId)
	{
		return await CaseTemplates.AsQueryable().FirstOrDefaultAsync(x => x.Id == templateId);
	}

	public async Task<List<CaseTemplate>> GetAllCaseTemplates()
	{
		return await CaseTemplates.AsQueryable().OrderByDescending(x => x.CreatedAt).ToListAsync();
	}

	public async Task<List<CaseTemplate>> GetAllTemplatesFromUser(ulong userId)
	{
		return await CaseTemplates.AsQueryable().Where(x => x.UserId == userId).OrderByDescending(x => x.CreatedAt)
			.ToListAsync();
	}

	public async Task<int> CountAllCaseTemplates()
	{
		return await CaseTemplates.AsQueryable().CountAsync();
	}

	public async Task SaveCaseTemplate(CaseTemplate template)
	{
		await CaseTemplates.AddAsync(template);
		await SaveChangesAsync();
	}

	public async Task DeleteSpecificCaseTemplate(CaseTemplate template)
	{
		CaseTemplates.Remove(template);
		await SaveChangesAsync();
	}

	public async Task DeleteAllTemplatesForGuild(ulong guildId)
	{
		var templates = await CaseTemplates.AsQueryable().Where(x => x.CreatedForGuildId == guildId).ToListAsync();
		CaseTemplates.RemoveRange(templates);
		await SaveChangesAsync();
	}


	public async Task<ModCaseComment> SelectSpecificModCaseComment(int commentId)
	{
		return await ModCaseComments.AsQueryable().FirstOrDefaultAsync(c => c.Id == commentId);
	}

	public async Task<List<ModCaseComment>> SelectLastModCaseCommentsByGuild(ulong guildId)
	{
		return await ModCaseComments.Include(x => x.ModCase).AsQueryable().Where(x => x.ModCase.GuildId == guildId)
			.OrderByDescending(x => x.CreatedAt).Take(10).ToListAsync();
	}

	public async Task<int> CountCommentsForGuild(ulong guildId)
	{
		return await ModCaseComments.Include(x => x.ModCase).AsQueryable().Where(x => x.ModCase.GuildId == guildId)
			.CountAsync();
	}

	public async Task SaveModCaseComment(ModCaseComment comment)
	{
		await ModCaseComments.AddAsync(comment);
		await SaveChangesAsync();
	}

	public async Task UpdateModCaseComment(ModCaseComment comment)
	{
		ModCaseComments.Update(comment);
		await SaveChangesAsync();
	}

	public async Task DeleteSpecificModCaseComment(ModCaseComment comment)
	{
		ModCaseComments.Remove(comment);
		await SaveChangesAsync();
	}

	public async Task<List<string>> GetAllLabels(ulong guildId)
	{
		return (await ModCases.AsQueryable().Where(x => x.GuildId == guildId).ToListAsync()).SelectMany(x => x.Labels).ToList();
	}

	public async Task<List<ModCase>> SelectAllModCasesMarkedAsDeleted()
	{
		return await ModCases.AsQueryable().Where(x => x.MarkedToDeleteAt < DateTime.UtcNow).ToListAsync();
	}

	public async Task<ModCase> SelectSpecificModCase(ulong guildId, int modCaseId)
	{
		return await ModCases.Include(c => c.Comments).AsQueryable()
			.FirstOrDefaultAsync(x => x.GuildId == guildId && x.CaseId == modCaseId);
	}

	public async Task<List<ModCase>> SelectAllModCasesForSpecificUserOnGuild(ulong guildId, ulong userId)
	{
		return await ModCases.AsQueryable().Where(x => x.GuildId == guildId && x.UserId == userId)
			.OrderByDescending(x => x.CaseId).ToListAsync();
	}
	
	public async Task<List<ModCase>> SelectAllModCasesForGuild(ulong guildId)
	{
		return await ModCases.AsQueryable().Where(x => x.GuildId == guildId).OrderByDescending(x => x.CaseId)
			.ToListAsync();
	}
	
	public async Task<List<ModCase>> SelectAllModCasesForSpecificUserOnGuild(ulong guildId, ulong userId, int startPage,
		int pageSize)
	{
		return await ModCases.AsQueryable().Where(x => x.GuildId == guildId && x.UserId == userId)
			.OrderByDescending(x => x.CaseId).Skip(startPage * pageSize).Take(pageSize).ToListAsync();
	}
	
	public async Task<List<ModCase>> SelectAllModCasesForGuild(ulong guildId, int startPage, int pageSize)
	{
		return await ModCases.AsQueryable().Where(x => x.GuildId == guildId).OrderByDescending(x => x.CaseId)
			.Skip(startPage * pageSize).Take(pageSize).ToListAsync();
	}
	
	public async Task<List<ModCase>> SelectAllModCasesWithActiveMuteForGuildAndUser(ulong guildId, ulong userId)
	{
		return await ModCases.AsQueryable().Where(x =>
			x.GuildId == guildId && x.UserId == userId && x.PunishmentActive == true &&
			x.PunishmentType == PunishmentType.Mute && x.MarkedToDeleteAt == null).ToListAsync();
	}

	public async Task<List<ModCase>> SelectAllModCasesWithActivePunishments()
	{
		return await ModCases.AsQueryable().Where(x => x.PunishmentActive == true && x.MarkedToDeleteAt == null)
			.ToListAsync();
	}
	
	public async Task<List<ModCase>> SelectAllModCasesForSpecificUser(ulong userId)
	{
		return await ModCases.AsQueryable().Where(x => x.UserId == userId).ToListAsync();
	}

	public async Task<List<ModCase>> SelectLatestModCases(DateTime timeLimit, int limit = 1000)
	{
		return await ModCases.AsQueryable().Where(x => x.CreatedAt >= timeLimit).OrderByDescending(x => x.CreatedAt)
			.Take(limit).ToListAsync();
	}

	public async Task<int> CountAllModCases()
	{
		return await ModCases.AsQueryable().CountAsync();
	}

	public async Task<int> CountAllModCasesForGuild(ulong guildId)
	{
		return await ModCases.AsQueryable().Where(x => x.GuildId == guildId).CountAsync();
	}

	public async Task<int> CountAllActivePunishmentsForGuild(ulong guildId)
	{
		return await ModCases.AsQueryable().Where(x => x.GuildId == guildId && x.PunishmentActive == true)
			.CountAsync();
	}

	public async Task<int> CountAllActivePunishmentsForGuild(ulong guildId, PunishmentType type)
	{
		return await ModCases.AsQueryable()
			.Where(x => x.GuildId == guildId && x.PunishmentActive == true && x.PunishmentType == type)
			.CountAsync();
	}

	public async Task<List<DbCount>> GetCaseCountGraph(ulong guildId, DateTime since)
	{
		return await ModCases.AsQueryable().Where(x => x.GuildId == guildId && x.OccurredAt > since)
			.GroupBy(x => new { x.OccurredAt.Month, x.OccurredAt.Year })
			.Select(x => new DbCount { Year = x.Key.Year, Month = x.Key.Month, Count = x.Count() })
			.OrderByDescending(x => x.Year).ThenByDescending(x => x.Month).ToListAsync();
	}

	public async Task<List<DbCount>> GetPunishmentCountGraph(ulong guildId, DateTime since)
	{
		return await ModCases.AsQueryable().Where(x =>
				x.GuildId == guildId && x.OccurredAt > since && x.PunishmentType != PunishmentType.Warn)
			.GroupBy(x => new { x.OccurredAt.Month, x.OccurredAt.Year })
			.Select(x => new DbCount { Year = x.Key.Year, Month = x.Key.Month, Count = x.Count() })
			.OrderByDescending(x => x.Year).ThenByDescending(x => x.Month).ToListAsync();
	}

	public async Task<int> GetHighestCaseIdForGuild(ulong guildId)
	{
		var query = ModCases.AsQueryable().Where(x => x.GuildId == guildId);

		if (!await query.AnyAsync())
			return 0;

		return await query.MaxAsync(p => p.CaseId);
	}

	public async Task DeleteAllModCasesForGuild(ulong guildId)
	{
		var cases = await ModCases.AsQueryable().Where(x => x.GuildId == guildId).ToListAsync();

		ModCases.RemoveRange(cases);
		await SaveChangesAsync();
	}

	public async Task DeleteSpecificModCase(ModCase modCase)
	{
		ModCases.Remove(modCase);
		await SaveChangesAsync();
	}

	public async Task UpdateModCase(ModCase modCase)
	{
		ModCases.Update(modCase);
		await SaveChangesAsync();
	}

	public async Task SaveModCase(ModCase modCase)
	{
		await ModCases.AddAsync(modCase);
		await SaveChangesAsync();
	}
}