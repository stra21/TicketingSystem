using System.Linq.Expressions;

namespace DL.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task<T> Create(T entity, CancellationToken cancellationToken);
        Task<T> Get(Expression<Func<T, bool>> expression, CancellationToken cancellationToken);
        Task<List<T>> GetList(Expression<Func<T, bool>> expression, CancellationToken cancellationToken);
        Task<bool> Delete(Expression<Func<T, bool>> expression, CancellationToken cancellationToken);
        Task<T> Update(T entity, CancellationToken cancellationToken);
    }
}
