using Authenticator_API.Data;
using Authenticator_API.Models;
using Authenticator_API.Models.HelperModels;
using System.Linq;
using System.Threading.Tasks;

namespace Authenticator_API.Services
{
    public class UserAuthenticateService : IUserAuthenticate
    {
        private readonly IUserDAO _userDAO;

        public UserAuthenticateService(IUserDAO userDAO)
        {
            _userDAO = userDAO;
        }

        public async Task<UserSensitive> AuthenticateAsync(AuthenticateUser model)
        {
            if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
                return null;

            var user = (await _userDAO.BuscarPorUsername(model.Username));

            if (user == null)
                return null;

            if (!HashServices.VerifyPasswordHash(model.Password, new PasswordHash { passwordHash = user.PasswordHash, passwordSalt = user.PasswordSalt }))
                return null;

            // authentication successful
            return new UserSensitive(user);
        }

        public async Task<UserSensitive> RegisterAsync(RegisterUser model)
        {
            if (string.IsNullOrWhiteSpace(model.Password))
                throw new AppException("Password is required");

            var Users = (await _userDAO.BuscarTodos()).ToList();

            if (Users.Any(x => x.Username == model.Username))
                throw new AppException("Username \"" + model.Username + "\" is already taken");

            var passwordEncrypted = HashServices.CreatePasswordEncrypted(model.Password);

            var user = new User()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Username = model.Username,
                PasswordHash = passwordEncrypted.passwordHash,
                PasswordSalt = passwordEncrypted.passwordSalt
            };

            _userDAO.Inserir(user);

            return new UserSensitive(user);
        }

        public Task<UserSensitive> RegisterAsync(AuthenticateUser model)
        {
            throw new System.NotImplementedException();
        }
    }
}
