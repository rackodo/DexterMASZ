using Bot.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PrivateVcs.Models;

namespace PrivateVcs.Data;

public class PrivateVcDatabase : DataContext<PrivateVcDatabase>, IDataContextCreate
{
    public PrivateVcDatabase(DbContextOptions<PrivateVcDatabase> options) : base(options)
    {
    }
    
    public DbSet<PrivateVcConfig> PrivateVcConfigs { get; set; }
    
    public static void AddContextToServiceProvider(Action<DbContextOptionsBuilder> optionsAction,
        IServiceCollection serviceCollection) =>
        serviceCollection.AddDbContext<PrivateVcDatabase>(optionsAction);

    public async Task<PrivateVcConfig> PutPrivateVcConfig(PrivateVcConfig pvcConfig)
    {
        PrivateVcConfigs.Update(pvcConfig);
        await SaveChangesAsync();
        return pvcConfig;
    }
}
