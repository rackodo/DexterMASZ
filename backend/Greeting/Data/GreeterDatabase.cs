using Bot.Abstractions;
using Greeting.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Greeting.Data;

public class GreeterDatabase(DbContextOptions<GreeterDatabase> options) :
    DataContext<GreeterDatabase>(options), IDataContextCreate
{
    public DbSet<GreetGateModel> GreeterConfigs { get; set; }

    public static void AddContextToServiceProvider(Action<DbContextOptionsBuilder> optionsAction,
        IServiceCollection serviceCollection) =>
        serviceCollection.AddDbContext<GreeterDatabase>(optionsAction);
}
