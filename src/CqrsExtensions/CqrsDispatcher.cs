using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace CqrsExtensions
{
    public class CqrsDispatcher : ICqrsDispatcher
    {
        private static readonly ConcurrentDictionary<Type, QueryDispatcherBase> QueryDispatchers = new();

        private readonly IServiceProvider _serviceProvider;

        public CqrsDispatcher(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

        public Task<TResult> Dispatch<TResult>(IQuery<TResult> query, CancellationToken cancellationToken)
        {
            var queryDispatcher = QueryDispatchers.GetOrAdd(query.GetType(), queryType =>
            {
                var makeGenericType = typeof(QueryDispatcher<,>).MakeGenericType(queryType, typeof(TResult));
                return ((QueryDispatcherBase) Activator.CreateInstance(makeGenericType))!;
            });
            return (Task<TResult>) queryDispatcher.Dispatch(query, _serviceProvider, cancellationToken);
        }
    }

    internal abstract class QueryDispatcherBase
    {
        public abstract object Dispatch(object query, IServiceProvider serviceProvider, CancellationToken cancellationToken);
    }

    internal class QueryDispatcher<TQuery, TResult> : QueryDispatcherBase where TQuery : IQuery<TResult>
    {
        public override object Dispatch(object query,IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            return Dispatch((TQuery) query, serviceProvider, cancellationToken);
        }

        private static async Task<TResult> Dispatch(TQuery query,IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            var serviceScope = serviceProvider.CreateScope();
            var queryHandler = serviceScope.ServiceProvider.GetRequiredService<IQueryHandler<TQuery, TResult>>();
            return await queryHandler.Handle(query, cancellationToken);
        }
    }
}