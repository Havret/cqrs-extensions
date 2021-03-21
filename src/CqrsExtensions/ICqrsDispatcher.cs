using System.Threading;
using System.Threading.Tasks;

namespace CqrsExtensions
{
    public interface ICqrsDispatcher
    {
        Task<TResult> Dispatch<TResult>(IQuery<TResult> query, CancellationToken cancellationToken);
    }
}