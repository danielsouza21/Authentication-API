using Authenticator_API.Models;
using Authenticator_API.Repositories;
using Authenticator_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Authenticator_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Auth : ControllerBase
    {
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
        public dynamic Authenticate([FromBody] User model)
        {
            var user = UserRepository.Get(model.Username, model.Password);

            if (user == null)
                return NotFound(new { message = "Usuário ou senha inválidos" });

            var token = TokenService.GenerateToken(user);

            user.Password = "";

            return new
            {
                user = user,
                token = token
            };
        }
    }
}
