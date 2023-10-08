using DL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BL.Interfaces
{
    public interface IUsersService
    {
        Task<List<Claim>> Authenticate(string username, string password, CancellationToken cancellationToken);
        Task<User> CreateUser(User entity, CancellationToken cancellationToken);
        Task<List<User>> GetUsers(Expression<Func<User,bool>> expression,CancellationToken cancellationToken);
        Task<User> GetUser(Expression<Func<User, bool>> expression, CancellationToken cancellationToken);
        Task<User> UpdateUser(User entity, CancellationToken cancellationToken);
    }
}
