using DL.Models;
using System.Linq.Expressions;

namespace BL.Interfaces
{
    public interface IModulesService
    {
        Task<List<Module>> Get(CancellationToken cancellationToken);
        Task<Module> Get(Expression<Func<Module, bool>> expression, CancellationToken cancellationToken);
    }
}
