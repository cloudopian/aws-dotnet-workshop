using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyPetApp.Web.Common;

namespace MyPetApp.Web.Areas.Identity.Pages.Account.Claims
{
    [Authorize]
    public class ShowClaimsModel : PageModel
    {
        public class UserClaimInfo
        {
            public IEnumerable<System.Security.Claims.Claim> UserClaims { get; set; }
            public string Username { get; set; }
        }
        public IEnumerable<UserClaimInfo> UserClaims
        {
            get
            {
                IEnumerable<Claim> claims = User.Claims;
                UserClaimInfo ui = new UserClaimInfo() { Username = User.Identity.Name, UserClaims = claims };
                return new List<UserClaimInfo>() { ui };
            }
        }
        public void OnGet()
        {
        }
    }
}