using Authenticator_API.Data;
using Authenticator_API.Models;
using Authenticator_API.Models.HelperModels;
using Authenticator_API.Services;
using AutoFixture;
using FluentAssertions;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Authenticator_Testes.ServicesTests
{
    public class UserAuthenticateTests
    {
        private Fixture _fixture;

        public UserAuthenticateTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public async Task GivenAValidUser_ShouldRegisterAsync()
        {
            var user = _fixture.Create<RegisterUser>();

            var userDAOMock = new Mock<IUserDAO>();
            userDAOMock.Setup(m => m.BuscarTodos()).Returns(Task.FromResult(Enumerable.Empty<User>()));
            userDAOMock.Setup(m => m.Inserir(It.IsAny<User>()));

            var _userAuthenticateService = new UserAuthenticateService(userDAOMock.Object);

            UserSensitive userResponse = await _userAuthenticateService.RegisterAsync(user);

            userResponse.Should().NotBeNull();
            userResponse.Username.Should().Be(user.Username);
        }

        [Fact]
        public async Task GivenAValidUser_ShouldAuthenticateAsync()
        {
            var password = "teste";
            var passwordEncrypted = HashServices.CreatePasswordEncrypted(password);

            var userModel = _fixture.Build<AuthenticateUser>()
                .With(u => u.Password, password)
                .Create();

            var user = _fixture.Build<User>()
                .With(u => u.PasswordHash, passwordEncrypted.passwordHash)
                .With(u => u.PasswordSalt, passwordEncrypted.passwordSalt)
                .Create();

            var userDAOMock = new Mock<IUserDAO>();
            userDAOMock.Setup(m => m.BuscarPorUsername(It.IsAny<string>())).Returns(Task.FromResult(user));

            var _userAuthenticateService = new UserAuthenticateService(userDAOMock.Object);

            UserSensitive userResponse = await _userAuthenticateService.AuthenticateAsync(userModel);

            userResponse.Should().NotBeNull();
            userResponse.Username.Should().Be(user.Username);
        }
    }
}
