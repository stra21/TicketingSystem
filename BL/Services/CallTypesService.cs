using BL.Interfaces;
using DL.Interfaces;
using DL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class CallTypesService : ICallTypesService
    {
        private readonly IBaseRepository<CallType> _baseRepository;
        public CallTypesService(IBaseRepository<CallType> baseRepository)
        {
            _baseRepository = baseRepository;
        }
        public async Task<List<CallType>> Get(CancellationToken cancellationToken)
        => await _baseRepository.GetList(x => true, cancellationToken);

        public async Task<CallType> Get(Expression<Func<CallType, bool>> expression, CancellationToken cancellationToken)
        => await _baseRepository.Get(expression, cancellationToken);
    }
}
