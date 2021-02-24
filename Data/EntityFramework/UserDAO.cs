using Authenticator_API.Models;
using System.Collections.Generic;

namespace Authenticator_API.Data.EntityFramework
{
    public class UserDAO : IUserDAO
    {
        public IEnumerable<User> Buscar()
        {
            throw new System.NotImplementedException();
        }

        public User BuscarPorId(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
