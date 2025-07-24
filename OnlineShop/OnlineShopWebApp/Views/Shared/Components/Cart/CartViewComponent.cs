using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.db;
using OnlineShop.db.Models;
using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Views.Shared.ViewComponents.CartViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        private readonly ICartsRepository cartsRepository;
        private readonly IMapper mapper;

        public CartViewComponent(ICartsRepository cartsRepository, IMapper mapper)
        {
            this.cartsRepository = cartsRepository;
            this.mapper = mapper;
        }
        public IViewComponentResult Invoke()
        {
            var cart = cartsRepository.TryGetByUserId(ConstantFields.UserId);
            var cartViewModel = mapper.Map<CartViewModel>(cart);
            var productCount = cartViewModel?.Quantity ?? 0;
            return View("Cart", productCount);
        }
    }
}
