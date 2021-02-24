using Authenticator_API.Models;
using System.Collections.Generic;

namespace Authenticator_API.Data
{
    public interface IUserDAO
    {
        public User BuscarPorId(int id);
        IEnumerable<User> Buscar();
    }
}
