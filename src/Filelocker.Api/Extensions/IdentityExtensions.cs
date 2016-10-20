using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace Filelocker.Api.Extensions
{
    public static class IdentityExtensions
    {
        public static int GetUserId(this IIdentity identity)
        {
            var claimsIdentity = (ClaimsIdentity)identity;
            var userId = int.Parse(claimsIdentity.Claims.Where(c => c.Type == "sub").Select(c => c.Value).SingleOrDefault());
            return userId;
        }
    }
}
