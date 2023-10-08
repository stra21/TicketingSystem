using DL.Interfaces;
using DL.Models;
using System.Linq.Expressions;

namespace BL.Interfaces
{
    public interface ICallsService
    {
        Task<List<Call>> GetCalls(CancellationToken cancellationToken);
        Task<Call> CreateCall(Call call,CancellationToken cancellationToken);
        Task<Call> GetCall(Expression<Func<Call,bool>> expression,CancellationToken cancellationToken);
        Task<Call> UpdateCall(Call call,CancellationToken cancellationToken);
        Task<bool> DeleteCall(long id,CancellationToken cancellationToken);
    }
}
