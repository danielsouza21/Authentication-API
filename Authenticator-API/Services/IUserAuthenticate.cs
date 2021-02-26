using Authenticator_API.Models;
using Authenticator_API.Models.HelperModels;
using System.Threading.Tasks;

namespace Authenticator_API.Services
{
    public interface IUserAuthenticate
    {
        public Task<UserSensitive> RegisterAsync(RegisterUser model);
        public Task<UserSensitive> AuthenticateAsync(AuthenticateUser model);
    }
}