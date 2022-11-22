using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Bot.Abstractions;

public interface IDataContextCreate : IDataContextInitialize
{
    public static abstract void AddContextToServiceProvider(
        Action<DbContextOptionsBuilder> optionsAction,
        IServiceCollection serviceCollection);
}
