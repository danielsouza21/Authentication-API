using Authenticator_API;
using Authenticator_API.Models;
using Authenticator_API.Models.HelperModels;
using Authenticator_API.Services;
using AutoFixture;
using FluentAssertions;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Principal;
using System.Text;
using Xunit;

namespace Authenticator_Testes.ServicesTests
{
    public class TokenServiceTest
    {
        private Fixture _fixture;

        public TokenServiceTest()
        {
            this._fixture = new Fixture();
        }

        [Fact]
        public void GenerateToken_GiveAValidUser_ShouldReturnAValidToken()
        {
            var userFixture = _fixture.Create<UserSensitive>();

            var token = TokenService.GenerateToken(userFixture);
            var validated = ValidateToken(token);

            token.Should().NotBeNullOrEmpty().And.BeOfType<string>();
            validated.Should().BeTrue();
        }

        //Private methods

        private static bool ValidateToken(string authToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = GetValidationParameters();

            SecurityToken validatedToken;
            try
            {
                IPrincipal principal = tokenHandler.ValidateToken(authToken, validationParameters, out validatedToken);
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        private static TokenValidationParameters GetValidationParameters()
        {
            var key = Encoding.ASCII.GetBytes(Settings.Secret);

            return new TokenValidationParameters()
            {
                ValidateLifetime = false, // Because there is no expiration in the generated token
                ValidateAudience = false, // Because there is no audiance in the generated token
                ValidateIssuer = false,   // Because there is no issuer in the generated token
                ValidIssuer = "Sample",
                ValidAudience = "Sample",
                IssuerSigningKey = new SymmetricSecurityKey(key) // The same key as the one that generate the token
            };
        }
    }
}
