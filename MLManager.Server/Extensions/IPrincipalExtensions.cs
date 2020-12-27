using System;
using System.Linq;
using System.Text.Json;
using System.Security.Principal;
using System.Security.Claims;

namespace MLManager.Controllers
{
    public static class IPrincipalExtensions
    {
        public static T GetClaimValue<T>(this IPrincipal principal,string name)
        {
            ClaimsIdentity identity = principal.Identity as ClaimsIdentity;
            string claimValue = identity.Claims.Where(x => x.Type == name).Select(x => x.Value).First();
            return JsonSerializer.Deserialize<T>(claimValue);
        }

        public static int GetUserId(this IPrincipal principal)
        {
            return principal.GetClaimValue<int>("UserId");
        }

        public static string GetFirstName(this IPrincipal principal)
        {
            return principal.GetClaimValue<string>("FirstName");
        }

        public static string GetLastName(this IPrincipal principal)
        {
            return principal.GetClaimValue<string>("LastName");
        }

        public static string GetUsername(this IPrincipal principal)
        {
            return principal.GetClaimValue<string>("Username");
        }
    }
}