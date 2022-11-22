using Bot.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Music.Models;

namespace Music.Data;

public class MusicDatabase : DataContext<MusicDatabase>, IDataContextCreate
{
    public DbSet<StartTime> StartTimes { get; set; }

    public MusicDatabase(DbContextOptions<MusicDatabase> options) : base(options)
    {
    }

    public static void AddContextToServiceProvider(Action<DbContextOptionsBuilder> optionsAction,
        IServiceCollection serviceCollection) =>
        serviceCollection.AddDbContext<MusicDatabase>(optionsAction);

    public async Task DeleteGuildData(ulong guildId)
    {
        var startTime = await StartTimes.FindAsync(guildId);
        if (startTime != null)
        {
            StartTimes.Remove(startTime);
            await SaveChangesAsync();
        }
    }

    public async Task SetGuildStartTime(ulong guildId, DateTime newTime)
    {
        var startTime = await StartTimes.FindAsync(guildId);

        if (startTime != null)
        {
            startTime.RadioStartTime = newTime;
        }
        else
        {
            startTime = new StartTime
            {
                Id = guildId,
                RadioStartTime = newTime
            };
            StartTimes.Add(startTime);
        }

        await SaveChangesAsync();
    }

    public async Task<StartTime> GetGuildStartTime(ulong guildId)
    {
        var startTime = await StartTimes.FindAsync(guildId);

        if (startTime == null)
        {
            startTime = new StartTime
            {
                Id = guildId,
                RadioStartTime = DateTime.Now
            };
            StartTimes.Add(startTime);
            await SaveChangesAsync();
        }

        return startTime;
    }
}
