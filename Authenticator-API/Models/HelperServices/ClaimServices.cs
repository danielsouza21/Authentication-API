using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Authenticator_API.Models.HelperServices
{
    public class ClaimServices
    {
        public static string GetValueFromClaimType(string type, ClaimsPrincipal User)
        {
            type = type.ToLower();

            var basetype = "http://schemas.microsoft.com/ws/2008/06/identity/claims/";

            var value = User.FindFirst(basetype + type).Value;

            return value;
        }
    }
}
