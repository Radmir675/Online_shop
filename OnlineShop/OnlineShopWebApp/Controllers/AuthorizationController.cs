using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.db.Models;
using OnlineShopWebApp.Models;
using System.Linq;

namespace OnlineShopWebApp.Controllers
{
    public class AuthorizationController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public AuthorizationController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        public IActionResult ShowAuthorizationForm(string returnUrl)
        {
            return View("Login", new UserLogin() { ReturnUrl = returnUrl });
        }
        public IActionResult ShowRegistrationForm(string returnUrl)
        {
            return View("Registration", new UserRegistration() { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public IActionResult Login(UserLogin userLogin, bool isRemember)
        {
            if (userLogin.Email == userLogin.Password)
            {
                ModelState.AddModelError("", "Password and login must not be the same");
                return View(userLogin);
            }
            if (!ModelState.IsValid)
            {
                return View(userLogin);
            }
            var result = signInManager.PasswordSignInAsync(userLogin.Email, userLogin.Password, false, false).Result;
            if (result.Succeeded)
            {
                return userLogin.ReturnUrl != null ? Redirect(userLogin.ReturnUrl) : View("SuccessPage", "You have successfully logged in");
            }
            else
            {
                ModelState.AddModelError("", "Login or password is wrong");
                return View(userLogin);
            }
        }

        public IActionResult Logout()
        {
            signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Registration(UserRegistration user)
        {
            if (user.Email == user.Password)
            {
                ModelState.AddModelError("", "Password and login must not be the same");
                return View(user);
            }
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            var searchedUser = userManager.FindByEmailAsync(user.Email).Result;
            if (searchedUser == null)
            {
                var newUser = new User { Email = user.Email, UserName = user.Email };
                var result = userManager.CreateAsync(newUser, user.Password).Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(newUser, ConstantFields.UserRoleName).Wait();
                    var loginResult = signInManager.PasswordSignInAsync(user.Email, user.Password, false, false).Result;
                    if (loginResult.Succeeded)
                    {
                        return user.ReturnUrl != null ? Redirect(user.ReturnUrl) : RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    var description = result.Errors.Select(x => x.Description).First();
                    ModelState.AddModelError("Password", description);
                }
            }
            else
            {
                ModelState.AddModelError("", "This email is registered");
            }
            return View(user);
        }
    }
}
