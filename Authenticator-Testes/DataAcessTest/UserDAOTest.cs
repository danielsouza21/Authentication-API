﻿using Authenticator_API.Data;
using Authenticator_API.Data.EntityFramework;
using Authenticator_API.Models;
using Authenticator_API.Services;
using AutoFixture;
using FluentAssertions;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Authenticator_Testes.DataAcessTest
{
    public class UserDAOTest
    {
        private Fixture _fixture;

        public UserDAOTest()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public async Task CouldSetAndGetUsersAsync()
        {
            using (var context = GetContextSetup())
            {
                var UserDAO = new UserDAO(context);
                var user = CreateUserModel();

                UserDAO.Inserir(user);
                var Users = (await UserDAO.Buscar()).ToList();

                Users.Should().NotBeEmpty().And.Contain(user);
            }
        }

        private User CreateUserModel()
        {
            var password = _fixture.Create<string>();
            var passwordEncrypted = HashServices.CreatePasswordEncrypted(password);

            var user = _fixture.Build<User>()
                .With(u => u.PasswordHash, passwordEncrypted.passwordHash)
                .With(u => u.PasswordSalt, passwordEncrypted.passwordSalt)
                .Without(u => u.Id)
                .Create();

            return user;
        }

        // Private methods

        private UserContext GetContextSetup()
        {
            var contextSetup = new SetupTestDb();
            return contextSetup.GetContext();
        }
    }
}
