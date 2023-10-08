using BL.Interfaces;
using DL.Interfaces;
using DL.Models;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;

namespace BL.Services
{
    public class UserService : IUsersService
    {
        private readonly IBaseRepository<User> _baseRepository;
        public UserService(IBaseRepository<User> baseRepository)
        {
            _baseRepository = baseRepository;
        }
        public async Task<List<Claim>> Authenticate(string username, string password, CancellationToken cancellationToken)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = md5.ComputeHash(bytes);
                StringBuilder hashedPass = new();
                foreach (var element in hashBytes)
                {
                    hashedPass.Append(element.ToString("X2"));
                }
                var res = await _baseRepository.Get(x => x.UserName.ToLower() == username.ToLower() && x.Password == hashedPass.ToString(), cancellationToken);
                if (res is null)
                {
                    return new List<Claim>();
                }
                var claims = new List<Claim>()
                {
                    new Claim("UserName",res.UserName),
                    new Claim("Type",res.UserType.ToString()),
                    new Claim("ID",res.Id.ToString())
                };
                return claims;
            }
        }

        public async Task<User> CreateUser(User entity, CancellationToken cancellationToken)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] bytes = Encoding.ASCII.GetBytes(entity.Password);
                byte[] hashBytes = md5.ComputeHash(bytes);
                StringBuilder hashedPass = new();
                foreach (var element in hashBytes)
                {
                    hashedPass.Append(element.ToString("X2"));
                }
                entity.Password = hashedPass.ToString();
                var res = await _baseRepository.Create(entity, cancellationToken);
                return entity;
            }
        }
        public async Task<User> UpdateUser(User entity, CancellationToken cancellationToken)
        {
            var oldUser = await _baseRepository.Get(x => x.Id == entity.Id, cancellationToken);
            if (oldUser is not null)
            {
                using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
                {
                    if (!string.IsNullOrEmpty(entity.Password))
                    {
                        byte[] bytes = Encoding.ASCII.GetBytes(entity.Password);
                        byte[] hashBytes = md5.ComputeHash(bytes);
                        StringBuilder hashedPass = new();
                        foreach (var element in hashBytes)
                        {
                            hashedPass.Append(element.ToString("X2"));
                        }
                        entity.Password = hashedPass.ToString();
                    }
                    else
                    {
                        entity.Password = oldUser.Password;
                    }
                }
            }
            return await _baseRepository.Update(entity, cancellationToken);
        }
        public async Task<List<User>> GetUsers(Expression<Func<User, bool>> expression, CancellationToken cancellationToken)
        => await _baseRepository.GetList(expression, cancellationToken);
        public async Task<User> GetUser(Expression<Func<User, bool>> expression, CancellationToken cancellationToken)
        => await _baseRepository.Get(expression, cancellationToken);
    }
}
