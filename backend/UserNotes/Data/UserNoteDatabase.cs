using Bot.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UserNotes.Models;

namespace UserNotes.Data;

public class UserNoteDatabase : DataContext<UserNoteDatabase>, IDataContextCreate
{
    public DbSet<UserNote> UserNotes { get; set; }

    public UserNoteDatabase(DbContextOptions<UserNoteDatabase> options) : base(options)
    {
    }

    public static void AddContextToServiceProvider(Action<DbContextOptionsBuilder> optionsAction,
        IServiceCollection serviceCollection) =>
        serviceCollection.AddDbContext<UserNoteDatabase>(optionsAction);

    public async Task<List<UserNote>> SelectLatestUserNotes(DateTime timeLimit, int limit) =>
        await UserNotes.AsQueryable().Where(x => x.UpdatedAt > timeLimit).OrderByDescending(x => x.UpdatedAt)
            .Take(limit).ToListAsync();

    public async Task<List<UserNote>> GetUserNotesByUserId(ulong userId) =>
        await UserNotes.AsQueryable().Where(x => x.UserId == userId).OrderByDescending(x => x.UpdatedAt)
            .ToListAsync();

    public async Task<List<UserNote>> GetUserNotesByGuildId(ulong guildId) =>
        await UserNotes.AsQueryable().Where(x => x.GuildId == guildId).OrderByDescending(x => x.UpdatedAt)
            .ToListAsync();

    public async Task<UserNote> GetUserNoteByUserIdAndGuildId(ulong userId, ulong guildId) =>
        await UserNotes.AsQueryable().Where(x => x.UserId == userId && x.GuildId == guildId)
            .FirstOrDefaultAsync();

    public async Task<int> CountUserNotes() => await UserNotes.AsQueryable().CountAsync();

    public async Task<int> CountUserNotesForGuild(ulong guildId) =>
        await UserNotes.AsQueryable().Where(x => x.GuildId == guildId).CountAsync();

    public async Task DeleteUserNote(UserNote userNote)
    {
        UserNotes.Remove(userNote);
        await SaveChangesAsync();
    }

    public async Task SaveUserNote(UserNote userNote)
    {
        UserNotes.Update(userNote);
        await SaveChangesAsync();
    }

    public async Task DeleteUserNoteByGuild(ulong guildId)
    {
        var userNotes = await UserNotes.AsQueryable().Where(x => x.GuildId == guildId).ToListAsync();
        UserNotes.RemoveRange(userNotes);
        await SaveChangesAsync();
    }
}
