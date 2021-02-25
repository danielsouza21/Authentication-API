using Authenticator_API.Models.HelperModels;
using Authenticator_API.Models.HelperServices;
using Authenticator_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

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

        [HttpGet]
        [Authorize]
        public string GetUserAuthenticated()
        {
            return $"User authenticated - Username '{ClaimServices.GetValueFromClaimType("NameIdentifier", User)}', Id = '{ClaimServices.GetValueFromClaimType("SerialNumber", User)}'";
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<dynamic> AuthenticateAsync([FromBody] AuthenticateUser model)
        {
            try
            {
                var user = await _userAuthenticate.AuthenticateAsync(model);

                if (user == null)
                    return BadRequest(new { message = "Username or password is incorrect" });

                var token = TokenService.GenerateToken(user);

                return Ok(new
                {
                    user = user,
                    token = token
                });
            }
            catch (ArgumentException arg)
            {
                return BadRequest(arg.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public async Task<dynamic> RegisterUserAsync([FromBody] RegisterUser model)
        {
            try
            {
                var user = await _userAuthenticate.RegisterAsync(model);
                var token = TokenService.GenerateToken(user);

                return Ok(new
                {
                    user = user,
                    token = token
                });
            }
            catch (AppException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
