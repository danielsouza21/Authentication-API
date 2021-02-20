using Authenticator_API.Models;
using Authenticator_API.Services;
using System.Collections.Generic;
using System.Linq;

namespace Authenticator_API.Data
{
    public class DbInitializer
    {
        public static void Initialize(UserContext context)
        {
            context.Database.EnsureCreated();

            if (context.Users.Any())
            {
                return;   // DB has been seeded
            }

            var UsersList = CreateUsers();

            foreach (User user in UsersList)
            {
                context.Users.Add(user);
            }

            context.SaveChanges();
        }

        private static IList<User> CreateUsers()
        {
            IList<User> UsersList = new List<User>();

            var passwordEncrypted = HashServices.CreatePasswordEncrypted("senha1");
            var user1 = new User { FirstName = "Daniel", LastName = "Souza", Username = "danielsz21", PasswordHash = passwordEncrypted.passwordHash, PasswordSalt = passwordEncrypted.passwordSalt };
            UsersList.Add(user1);

            passwordEncrypted = HashServices.CreatePasswordEncrypted("senha1");
            var user2 = new User { FirstName = "Daniel", LastName = "Souza", Username = "danielsz21", PasswordHash = passwordEncrypted.passwordHash, PasswordSalt = passwordEncrypted.passwordSalt };
            UsersList.Add(user2);

            passwordEncrypted = HashServices.CreatePasswordEncrypted("senha1");
            var user3 = new User { FirstName = "Daniel", LastName = "Souza", Username = "danielsz21", PasswordHash = passwordEncrypted.passwordHash, PasswordSalt = passwordEncrypted.passwordSalt };
            UsersList.Add(user3);

            return UsersList;
        }
    }
}
