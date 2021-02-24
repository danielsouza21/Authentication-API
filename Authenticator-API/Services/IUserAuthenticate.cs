using Authenticator_API.Models;

namespace Authenticator_API.Services
{
    public interface IUserAuthenticate
    {
        public User Authenticate(string Username, string Password);
    }
}