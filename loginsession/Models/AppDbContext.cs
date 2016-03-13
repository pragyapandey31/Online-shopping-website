using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace loginsession.Models
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public IDbSet<IdentityUserLogin> UserLogins { get; set; }
        public AppDbContext()
            : base("UsersDB")
        {
        }
    }
}