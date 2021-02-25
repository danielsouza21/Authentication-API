using Authenticator_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Authenticator_API.Data
{
    public interface IUserDAO
    {
        public Task<User> BuscarPorId(int id);
        Task<IEnumerable<User>> BuscarTodos();
        public Task<User> BuscarPorUsername(string Username);
        public void Inserir(User user);
    }
}
