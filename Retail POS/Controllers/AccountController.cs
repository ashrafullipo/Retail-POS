using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Retail_POS.Models;
using Retail_POS.ViewModel;
using Retail_POS.ViewMoodel;

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
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
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



        //Register
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // check if email already exists
                var existingUser = await userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    TempData["Message"] = "Email already registered!";
                    return View(model);
                }

                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FullName = model.FullName
                };

                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Default role: User
                    if (!await roleManager.RoleExistsAsync("User"))
                    {
                        await roleManager.CreateAsync(new IdentityRole("User"));
                    }

                    await userManager.AddToRoleAsync(user, "User");

                    // ✅ After successful registration, redirect to Login page
                    TempData["Message"] = "Registration successful! Please login to continue.";
                    return RedirectToAction("Login", "Account");
                }

                // show error messages
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            TempData["Message"] = "Registration failed. Please try again.";
            return View(model);
        }


        //ChangePassword
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ChangePassword() => View();

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    TempData["Message"] = "No account found with this email.";
                    return View(model);
                }

                var resetToken = await userManager.GeneratePasswordResetTokenAsync(user);   

                var res = await userManager.ResetPasswordAsync(user, resetToken, model.NewPassword);

                if (res.Succeeded)
                {
                    TempData["Message"] = "Password changed successfully! Please login with your new password.";
                    return RedirectToAction("Login", "Account");
                }

                foreach (var error in res.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            TempData["Message"] = "Password change failed. Please try again.";
            return View(model);
        }

        //VerifyEmaikl
        [HttpGet]
        [AllowAnonymous]
        public IActionResult VerifyEmail() => View();

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyEmail(VerifyEmailViewModel model)
        {
            if(ModelState.IsValid)
            {
                return View(model);
            }
            TempData["Message"] = "Verification link has been sent to your email.";
            return RedirectToAction("Login"); // login page e redirect
        }

    }
}
