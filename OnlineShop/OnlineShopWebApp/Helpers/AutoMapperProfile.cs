using AutoMapper;
using OnlineShop.db;

namespace OnlineShopWebApp.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ProductViewModel, Product>();
            CreateMap<Product, ProductViewModel>();

        }
    }
}
