using Authenticator_API.Data;
using Authenticator_API.Models;

namespace Authenticator_API.Services
{
    public class UserAuthenticateService : IUserAuthenticate
    {
        private readonly IUserDAO _userDAO;

        public UserAuthenticateService(IUserDAO userDAO)
        {
            _userDAO = userDAO;
        }

        public User Authenticate(string Username, string Password)
        {
            throw new System.NotImplementedException();
        }
    }
}
