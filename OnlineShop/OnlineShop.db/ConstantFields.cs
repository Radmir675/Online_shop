using System;

namespace OnlineShopWebApp.Models
{
    public static class ConstantFields
    {
        public static Guid UserId { get; set; } = Guid.NewGuid();
        public const string UserRoleName = "User";
        public const string AdminRoleName = "Admin";
        public const string KeyCacheAllProducts = "GetAllProducts";

    }
}
