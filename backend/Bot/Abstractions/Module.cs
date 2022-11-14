using Bot.Models;
using Bot.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Bot.Abstractions;

public abstract class Module
{
    public abstract string[] Contributors { get; }

    public virtual void AddLogging(ILoggingBuilder loggingBuilder)
    {
    }

    public virtual void AddPreServices(IServiceCollection services, CachedServices cachedServices,
        Action<DbContextOptionsBuilder> dbOption)
    {
    }

    public virtual void AddServices(IServiceCollection services, CachedServices cachedServices, AppSettings appSettings)
    {
    }

    public virtual void ConfigureModules(List<Module> modules, WebApplication app)
    {
    }

    public virtual void ConfigureServices(ConfigurationManager configuration, IServiceCollection services)
    {
    }

    public virtual void PostBuild(IServiceProvider services, CachedServices cachedServices)
    {
    }
}