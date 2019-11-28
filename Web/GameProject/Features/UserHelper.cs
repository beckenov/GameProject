using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace GnsGameProject.Features
{
    public static class UserHelper
    {
        public static Guid GetUserId(this IPrincipal principal)
        {
            var claimsIdentity = (ClaimsIdentity)principal.Identity;
            var claim = claimsIdentity.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            return Guid.Parse(claim.Value);
        }
    }
}
