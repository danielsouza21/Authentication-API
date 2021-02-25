using Authenticator_API.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Authenticator_API.Data.EntityFramework
{
    public class UserDAO : IUserDAO
    {
        private readonly UserContext _context;

        public UserDAO(UserContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> BuscarTodos()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> BuscarPorId(int id)
        {
            return await _context.Users
                .FirstAsync(m => m.Id == id);
        }

        public async Task<User> BuscarPorUsername(string Username)
        {
            return await _context.Users
                .FirstAsync(m => m.Username == Username);
        }

        public void Inserir(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }
    }
}
