using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.db.Interfaces;
using OnlineShop.db.Models;
using OnlineShopWebApp.Models;
using System.Threading.Tasks;

namespace OnlineShopWebApp.Views.Shared.Components.Cart
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
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var cart = await cartsRepository.TryGetByUserIdAsync(ConstantFields.UserId);
            var cartViewModel = mapper.Map<CartViewModel>(cart);
            var productCount = cartViewModel?.Quantity ?? 0;
            return View("Cart", productCount);
        }
    }
}
