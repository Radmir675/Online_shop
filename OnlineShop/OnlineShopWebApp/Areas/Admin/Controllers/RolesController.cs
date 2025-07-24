using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Models;
using System.Linq;

namespace OnlineShopWebApp.Areas.Admin.Controllers
{
    [Area(ConstantFields.AdminRoleName)]
    [Authorize(Roles = ConstantFields.AdminRoleName)]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IMapper mapper;

        public RolesController(RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            this.roleManager = roleManager;
            this.mapper = mapper;
        }
        public IActionResult ShowRoles()
        {
            var roles = roleManager.Roles;
            var rolesToView = roles.Select(role => mapper.Map<RoleViewModel>(role)).ToList();
            return View("Roles", rolesToView);
        }

        public IActionResult AddRole()
        {
            return View("NewRole");
        }

        [HttpPost]
        public IActionResult Save(RoleViewModel role)
        {
            var result = roleManager.CreateAsync(new IdentityRole { Name = role.Name }).Result;
            if (result.Succeeded)
            {
                return RedirectToAction("ShowRoles");
            }
            else
            {
                ModelState.AddModelError("", "This role exists");
            }
            return View("NewRole", role);
        }

        [HttpPost]
        public IActionResult Delete(string name)
        {
            var role = roleManager.FindByNameAsync(name).Result;
            if (role != null)
            {
                roleManager.DeleteAsync(role);
            }
            return RedirectToAction("ShowRoles");
        }
    }
}
