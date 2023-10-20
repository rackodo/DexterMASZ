using Bot.Abstractions;
using JoinLeave.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace JoinLeave.Data;

public class JoinLeaveDatabase(DbContextOptions<JoinLeaveDatabase> options) :
    DataContext<JoinLeaveDatabase>(options), IDataContextCreate
{
    public DbSet<JoinLeaveConfig> JoinLeaveConfig { get; set; }

    public static void AddContextToServiceProvider(Action<DbContextOptionsBuilder> optionsAction,
        IServiceCollection serviceCollection) =>
        serviceCollection.AddDbContext<JoinLeaveDatabase>(optionsAction);
}
