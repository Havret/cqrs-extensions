using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CqrsExtensions
{
    internal class CqrsDispatcher : ICqrsDispatcher
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<Type, QueryDispatcherBase> _queryDispatchers;

        public CqrsDispatcher(IServiceProvider serviceProvider, Dictionary<Type, QueryDispatcherBase> queryDispatchers)
        {
            _serviceProvider = serviceProvider;
            _queryDispatchers = queryDispatchers;
        }

        public Task<TResult> Dispatch<TResult>(IQuery<TResult> query, CancellationToken cancellationToken)
        {
            return (Task<TResult>) _queryDispatchers[query.GetType()].Dispatch(query, _serviceProvider, cancellationToken);
        }
    }
}