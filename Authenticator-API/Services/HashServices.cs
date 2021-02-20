using Authenticator_API.Models.HelperModels;
using System;

namespace Authenticator_API.Services
{
    public class HashServices
    {
        public static PasswordHash CreatePasswordEncrypted(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password is required");

            return CreatePasswordHash(password);
        }

        public static bool VerifyPasswordHash(string password, PasswordHash inputEncrypted)
        {
            var storedSalt = inputEncrypted.passwordSalt;
            var storedHash = inputEncrypted.passwordHash;

            ValidateArguments(password, storedHash, storedSalt);

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }

        // Private methods

        private static void ValidateArguments(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");
        }

        private static PasswordHash CreatePasswordHash(string password)
        {
            byte[] passwordHash, passwordSalt;
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }

            return new PasswordHash { passwordHash = passwordHash, passwordSalt = passwordSalt };
        }
    }
}
