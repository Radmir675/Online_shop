using Microsoft.AspNetCore.Identity;
using OnlineShop.db.Models;
using OnlineShopWebApp.Models;

namespace OnlineShop.db
{
    public class IdentityInitializer
    {
        public static void Initialize(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            var adminEmail = "admin@gmail.com";
            var password = "123456aA_";
            if (roleManager.FindByNameAsync(ConstantFields.AdminRoleName).Result == null)
            {
                roleManager.CreateAsync(new IdentityRole(ConstantFields.AdminRoleName)).Wait();
            }
            if (roleManager.FindByNameAsync(ConstantFields.UserRoleName).Result == null)
            {
                roleManager.CreateAsync(new IdentityRole(ConstantFields.UserRoleName)).Wait();
            }
            if (userManager.FindByNameAsync(adminEmail).Result == null)
            {
                var admin = new User { Email = adminEmail, UserName = adminEmail };
                var result = userManager.CreateAsync(admin, password).Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(admin, ConstantFields.AdminRoleName).Wait();
                }
            }
        }
    }
}
