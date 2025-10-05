using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Retail_POS.Models;
using Retail_POS.ViewModel;

namespace Retail_POS.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }


        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        //Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (model != null)
            {
                var usr = await userManager.FindByEmailAsync(model.Email);
                if (usr != null)
                {
                    var res = await signInManager.PasswordSignInAsync(usr, model.Password, false, false);
                    if (res.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["Message"] = "Invalid Password";
                    }
                }
                else
                {
                    TempData["Message"] = "Invalid Email";
                }
            }
            else
            {
                TempData["Message"] = "Something went wrong";
            }

            return View(model);
        }

        //Logout start here
        public async Task<IActionResult> Logout()
        {
            if(signInManager.IsSignedIn(User))
            {
                await signInManager.SignOutAsync();
            }
            return RedirectToAction("Login", "Account");
        }
        //Logout end here



        public IActionResult Register()
        {
            return View();
        }

       


        //Register
        
    }
}
