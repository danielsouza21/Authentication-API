using Authenticator_API.Data;
using Authenticator_API.Models.HelperModels;
using Authenticator_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authenticator_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserAuthenticate _userAuthenticate;

        public AuthController(IUserAuthenticate userAuthenticate)
        {
            _userAuthenticate = userAuthenticate;
        }

        // GET: api/<Auth>
        [HttpGet]
        [Authorize]
        public string GetUserAuthenticated()
        {
            return $"User authenticated - '{User.Identity.Name}'";
        }

        // POST api/<Auth>/login
        [HttpPost]
        [Route("login")]
        public dynamic Authenticate([FromBody] AuthenticateUser model)
        {
            var user = _userAuthenticate.Authenticate(model.Username, model.Password);

            if (user == null)
                return NotFound(new { message = "Usuário ou senha inválidos" });

            var token = TokenService.GenerateToken(user);

            return new
            {
                user = user,
                token = token
            };
        }
    }
}
