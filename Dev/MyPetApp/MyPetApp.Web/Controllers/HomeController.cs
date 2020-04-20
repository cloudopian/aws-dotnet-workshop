using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPetApp.Web.Models;

namespace MyPetApp.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        [Authorize]
        public IActionResult ManageClaims()
        {
           bool a= User.IsInRole("CatOwners");
            var claims = this.User.Claims;
           
            return View(claims);
        }


        [Authorize(Roles ="PetOwners")]
        public IActionResult ShowMyClaims()
        {
            var claims = this.User.Claims;

            return View(claims);
        }
        


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
