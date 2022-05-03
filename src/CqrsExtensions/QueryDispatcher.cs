using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace CqrsExtensions
{
    internal class QueryDispatcher<TQuery, TResult> : QueryDispatcherBase where TQuery : IQuery<TResult>
    {
        public override object Dispatch(object query, IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            return Dispatch((TQuery) query, serviceProvider, cancellationToken);
        }

        private static async Task<TResult> Dispatch(TQuery query, IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            using var serviceScope = serviceProvider.CreateScope();
            var queryHandler = serviceScope.ServiceProvider.GetRequiredService<IQueryHandler<TQuery, TResult>>();
            return await queryHandler.HandleAsync(query, cancellationToken);
        }
    }
}