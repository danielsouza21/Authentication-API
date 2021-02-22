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

        public async Task<IEnumerable<User>> Buscar()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> BuscarPorId(int id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public void Inserir(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }
    }
}
