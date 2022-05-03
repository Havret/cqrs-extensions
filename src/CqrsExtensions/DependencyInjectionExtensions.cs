using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CqrsExtensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddCqrs(this IServiceCollection serviceCollection, Assembly assembly)
        {
            var queryDispatchers = new Dictionary<Type, QueryDispatcherBase>();

            foreach (var type in assembly.GetTypes())
            {
                foreach (var @interface in type.GetInterfaces())
                {
                    var genericTypeDefinition = @interface.GetGenericTypeDefinition();
                    if (genericTypeDefinition == typeof(IQueryHandler<,>))
                    {
                        serviceCollection.TryAddScoped(@interface, type);
                        
                        var queryType = @interface.GetGenericArguments().First();
                        var queryResultType = @interface.GetGenericArguments().Last();
                        
                        var queryDispatcherType = typeof(QueryDispatcher<,>).MakeGenericType(queryType, queryResultType);
                        var queryDispatcher = (QueryDispatcherBase) Activator.CreateInstance(queryDispatcherType)!;

                        queryDispatchers.Add(queryType, queryDispatcher);
                    }
                }
            }
            
            serviceCollection.AddScoped<ICqrsDispatcher>(provider => new CqrsDispatcher(provider, queryDispatchers));

            return serviceCollection;
        }
    }
}