using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.db.Models;
using OnlineShopWebApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace OnlineShopWebApp.Areas.Admin.Controllers
{
    [Area(ConstantFields.AdminRoleName)]
    [Authorize(Roles = ConstantFields.AdminRoleName)]
    public class UsersController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<User> signInManager;

        public UsersController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
        }
        public IActionResult ShowUsers()
        {
            var users = userManager.Users?.ToList();
            var usersToView = users?.Select(x => GetUserViewModel(x)).ToList()
                ?? new List<UserViewModel>();

            return View("Users", usersToView);
        }
        private UserViewModel GetUserViewModel(User user)
        {
            return new UserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Role = userManager.GetRolesAsync(user).Result.First().ToString()
            };
        }

        [HttpPost]
        public IActionResult ShowInfo(string userId)
        {
            var user = userManager.FindByIdAsync(userId).Result;
            if (user == null)
            {
                return RedirectToAction("ShowUsers");
            }
            var userViewModel = GetUserViewModel(user);
            return View("UserInfo", userViewModel);
        }

        [HttpPost]
        public IActionResult Delete(string userId)
        {
            var user = userManager.FindByIdAsync(userId).Result;
            if (user != null)
            {
                var result = userManager.DeleteAsync(user).Result;
                if (result.Succeeded)
                {
                    return RedirectToAction("ShowUsers");
                }
            }
            return RedirectToAction("ShowUsers");
        }

        public IActionResult ShowNewUserForm()
        {
            return View("NewUser");
        }

        [HttpPost]
        public IActionResult AddUser(UserRegistration user)
        {
            if (user.Email == user.Password)
            {
                ModelState.AddModelError("Password", "Password and login must not be the same");
                return View("NewUser", user);
            }

            if (user.Password != user.PasswordConfirm)
            {
                ModelState.AddModelError("Password", "Password and Password confirmation must be the same");
                return View("NewUser", user);
            }

            if (userManager.Users.Any(x => x.Email == user.Email))
            {
                ModelState.AddModelError("Email", "This email is registered");
                return View("NewUser", user);
            }

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Something goes wrong!");
                return View("NewUser", user);
            }
            var searchedUser = userManager.FindByEmailAsync(user.Email).Result;
            if (searchedUser == null)
            {
                var newUser = new User { Email = user.Email, UserName = user.Email };
                var result = userManager.CreateAsync(newUser, user.Password).Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(newUser, ConstantFields.UserRoleName).Wait();
                    return RedirectToAction("ShowUsers");
                }
                else
                {
                    var description = result.Errors.Select(x => x.Description).First();
                    ModelState.AddModelError("", description);
                }
            }
            else
            {
                ModelState.AddModelError("", "This email is registered");
            }
            return View("NewUser", user);
        }

        [HttpPost]
        public IActionResult ChangePassword()
        {

            //данный раздел на стадии разработки

            return RedirectToAction("ShowUsers");
        }
        public IActionResult EditUserData()
        {
            //данный раздел на стадии разработки

            return RedirectToAction("ShowUsers");
        }
    }
}
