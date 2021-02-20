using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authenticator_API.Models.HelperModels
{
    public class PasswordHash
    {
        public byte[] passwordHash;
        public byte[] passwordSalt;
    }
}
