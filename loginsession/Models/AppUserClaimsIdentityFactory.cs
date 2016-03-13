using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace loginsession.Models
{
    public class AppUserClaimsIdentityFactory : ClaimsIdentityFactory<AppUser,string>
    {
        public override async Task<ClaimsIdentity> CreateAsync(
            UserManager<AppUser,string> manager,
            AppUser user,
            string authenticationType)
        {
            var identity = await base.CreateAsync(manager, user, authenticationType);
         //   identity.AddClaim(new Claim(ClaimTypes.Country, user.Country));

            return identity;
        }
    }

}