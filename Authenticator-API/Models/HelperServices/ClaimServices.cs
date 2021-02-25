using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Authenticator_API.Models.HelperServices
{
    public class ClaimServices
    {
        public static int GetIdFromClaim(ClaimsPrincipal User)
        {
            var value = User.FindAll(c => c.Type == ClaimTypes.SerialNumber)
                   .Select(c => c.Value).SingleOrDefault();

            return Int32.Parse(value);
        }

        public static string GetUsernameFromClaim(ClaimsPrincipal User)
        {
            var value = User.FindAll(c => c.Type == ClaimTypes.NameIdentifier)
                   .Select(c => c.Value).SingleOrDefault();

            return value;
        }
    }
}
