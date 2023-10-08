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
    public class ModulesService : IModulesService
    {
        private readonly IBaseRepository<Module> _baseRepository;
        public ModulesService(IBaseRepository<Module> baseRepository)
        {
            _baseRepository = baseRepository;
        }
        public async Task<List<Module>> Get(CancellationToken cancellationToken)
        => await _baseRepository.GetList(x=>true,cancellationToken);

        public async Task<Module> Get(Expression<Func<Module, bool>> expression, CancellationToken cancellationToken)
        => await _baseRepository.Get(expression,cancellationToken);
    }
}
