using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace CqrsExtensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddCqrs(this IServiceCollection serviceCollection, Assembly assembly)
        {
            serviceCollection.AddScoped<ICqrsDispatcher, CqrsDispatcher>();
            
            foreach (var type in assembly.GetTypes())
            {
                foreach (var @interface in type.GetInterfaces())
                {
                    if (@interface.GetGenericTypeDefinition() == typeof(IQueryHandler<,>))
                    {
                        serviceCollection.AddScoped(@interface, type);
                    }
                }
            }
            return serviceCollection;
        }
    }
}