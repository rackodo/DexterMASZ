using MASZ.Bot.Models;
using MASZ.Bot.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MASZ.Bot.Abstractions;

public abstract class Module
{
	public abstract string Maintainer { get; }

	public abstract string[] Contributors { get; }

	public abstract string[] Translators { get; }

	public virtual void AddLogging(ILoggingBuilder loggingBuilder)
	{
	}

	public virtual void AddPreServices(IServiceCollection services, ServiceCacher serviceCacher,
		Action<DbContextOptionsBuilder> dbOption)
	{
	}

	public virtual void AddServices(IServiceCollection services, ServiceCacher serviceCacher, AppSettings appSettings)
	{
	}

	public virtual void ConfigureServices(ConfigurationManager configuration, IServiceCollection services)
	{
	}

	public virtual void PostBuild(IServiceProvider services, ServiceCacher serviceCacher)
	{
	}
}