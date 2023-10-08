using DL.Interfaces;
using DL.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DL.Models
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly TicketingSystemContext _ctx;
        public BaseRepository(TicketingSystemContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<T> Create(T entity, CancellationToken cancellationToken)
        {
            var res = await _ctx.Set<T>().AddAsync(entity, cancellationToken);
            await _ctx.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<bool> Delete(Expression<Func<T, bool>> expression, CancellationToken cancellationToken)
        {
            var res = await _ctx.Set<T>().Where(expression).ToListAsync(cancellationToken);
            if (res is not null)
            {
                _ctx.RemoveRange(res);
                var result = await _ctx.SaveChangesAsync(cancellationToken);
                return result > 0;
            }
            return true;
        }

        public async Task<T> Get(Expression<Func<T, bool>> expression, CancellationToken cancellationToken)
        {
            var res = await _ctx.Set<T>().Where(expression).AsNoTracking().FirstOrDefaultAsync(cancellationToken);
            return res;
        }

        public async Task<List<T>> GetList(Expression<Func<T, bool>> expression, CancellationToken cancellationToken)
        {
            var res = await _ctx.Set<T>().Where(expression).ToListAsync(cancellationToken);
            return res;
        }

        public async Task<T> Update(T entity, CancellationToken cancellationToken)
        {
            var res = _ctx.Set<T>().Update(entity);
            await _ctx.SaveChangesAsync(cancellationToken);
            return entity;
        }
    }
}
