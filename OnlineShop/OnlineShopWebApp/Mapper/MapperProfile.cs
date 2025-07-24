using AutoMapper;
using Microsoft.AspNetCore.Identity;
using OnlineShop.db.Models;
using OnlineShopWebApp.Models;

namespace OnlineShop.db
{
    class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CartItem, CartItemViewModel>().ForMember("Product", x => x.MapFrom(x => x.Product)).ReverseMap();

            CreateMap<OrderViewModel, Order>()
                .ConvertUsing<OrderWithAdditionalParameters>();

            CreateMap<Order, OrderViewModel>().ForMember("CartItems", x => x.MapFrom(x => x.CartItems));

            CreateMap<Cart, CartViewModel>().ForMember("CartItems", x => x.MapFrom(x => x.CartItems));

            CreateMap<IdentityRole, RoleViewModel>().ForMember("Name", x => x.MapFrom(x => x.Name));
        }
    }
}
