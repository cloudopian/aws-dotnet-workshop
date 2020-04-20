using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyPetApp.Web.Controllers
{
    public class FoodController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "CatOwners")]
        public IActionResult CatFood()
        {
            return View();
        }

        [Authorize(Roles = "DogOwners")]
        public IActionResult DogFood()
        {
            return View();
        }
    }
}