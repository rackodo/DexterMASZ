using Bot.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Bot.Services;

public class CachedServices
{
    public readonly List<Assembly> Dependents;
    public readonly Dictionary<string, Type[]> Services;

    public CachedServices()
    {
        Services = [];

        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        Dependents = assemblies
            .Where(a => a.GetReferencedAssemblies()
                .Select(assemblyName => assemblyName.FullName)
                .Contains(Assembly.GetExecutingAssembly().FullName)).ToList();

        Dependents.Add(Assembly.GetExecutingAssembly());
    }

    public Type[] GetClassTypes<T>()
    {
        var type = typeof(T);

        if (type.FullName == null)
            return Type.EmptyTypes;

        if (Services.TryGetValue(type.FullName, out var value))
            return value;

        List<Type> classes = [];

        foreach (var assembly in Dependents)
        {
            classes.AddRange(
                assembly.GetTypes()
                    .Where(c => c.IsClass && (!c.IsAbstract && c.IsSubclassOf(type) ||
                                              !c.IsInterface && c.GetInterfaces().Contains(type)))
            );
        }

        Services.Add(type.FullName, [.. classes]);

        return [.. classes];
    }

    public List<T> GetInitializedClasses<T>(IServiceProvider serviceProvider) where T : class =>
        GetClassTypes<T>().Select(t => serviceProvider.GetRequiredService(t) as T).ToList();

    public List<T> GetInitializedAuthenticatedClasses<T>(IServiceProvider serviceProvider, Identity identity)
        where T : class =>
        GetClassTypes<T>().Select(t =>
        {
            var service = serviceProvider.GetRequiredService(t) as T;

            if (service is Repository authRepo)
                authRepo.AsUser(identity);

            return serviceProvider.GetRequiredService(t) as T;
        }).ToList();
}
