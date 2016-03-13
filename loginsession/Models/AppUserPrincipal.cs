using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace loginsession.Models
{
    public class AppUserPrincipal:ClaimsPrincipal
    {
        public AppUserPrincipal(System.Security.Claims.ClaimsPrincipal principal)
            : base(principal)
        {
        }

        public string Name
        {
            get
            {
                return this.FindFirst(ClaimTypes.Name).Value;
            }
        }

      //  public string Country
        //{
          //get
            //{
              //  if (this.FindFirst(ClaimTypes.Country).Value != null)
                //{
                  //  return this.FindFirst(ClaimTypes.Country).Value;
                //}
          //      return null;
            //}
        //}
    }
}