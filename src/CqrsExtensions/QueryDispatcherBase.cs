using System;
using System.Threading;

namespace CqrsExtensions
{
    internal abstract class QueryDispatcherBase
    {
        public abstract object Dispatch(object query, IServiceProvider serviceProvider, CancellationToken cancellationToken);
    }
}