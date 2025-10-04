using Microsoft.AspNetCore.Mvc;

namespace Retail_POS.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //Login
        public IActionResult Login()
        {
            return View();
        }


        //Register
        public IActionResult Register()
        {
            return View();
        }
    }
}
