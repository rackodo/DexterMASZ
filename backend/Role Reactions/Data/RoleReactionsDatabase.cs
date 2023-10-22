using Bot.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RoleReactions.Models;

namespace RoleReactions.Data;

public class RoleReactionsDatabase(DbContextOptions<RoleReactionsDatabase> options) :
    DataContext<RoleReactionsDatabase>(options), IDataContextCreate
{
    public DbSet<RoleMenu> RoleReactionsMenu { get; set; }
    public DbSet<UserRoles> UserRoles { get; set; }

    public static void AddContextToServiceProvider(Action<DbContextOptionsBuilder> optionsAction,
        IServiceCollection serviceCollection) =>
        serviceCollection.AddDbContext<RoleReactionsDatabase>(optionsAction);

    public override void OverrideModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<RoleMenu>()
            .Property(e => e.Roles)
            .HasConversion(new DictionaryDataConverter<string, ulong>(),
                new DictionaryDataComparer<string, ulong>());

        modelBuilder
            .Entity<RoleMenu>()
            .Property(e => e.Emotes)
            .HasConversion(new DictionaryDataConverter<string, string>(),
                new DictionaryDataComparer<string, string>());

        modelBuilder
            .Entity<UserRoles>()
            .Property(e => e.RoleIds)
            .HasConversion(new ListDataConverter<ulong>(),
                new ListDataComparer<ulong>());
    }
}
