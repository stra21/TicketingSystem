using DL.Models;
using System.Linq.Expressions;

namespace BL.Interfaces
{
    public interface ICallTypesService
    {
        Task<List<CallType>> Get(CancellationToken cancellationToken);
        Task<CallType> Get(Expression<Func<CallType, bool>> expression, CancellationToken cancellationToken);
    }
}
