using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.db.Interfaces;
using OnlineShopWebApp.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopWebApp.Controllers
{
    [Authorize]
    public class PurchasesController : Controller
    {
        private readonly ICartsRepository cartsDbRepository;
        private readonly IProductsRepository productsDbRepository;
        private readonly IMapper mapper;
        private Guid UserId = ConstantFields.UserId;
        public PurchasesController(ICartsRepository cartsDbRepository, IProductsRepository productsRepository, IMapper mapper)
        {
            this.cartsDbRepository = cartsDbRepository;
            this.productsDbRepository = productsRepository;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index(Guid? productId)
        {
            if (productId.HasValue)
            {
                var cart = await cartsDbRepository.TryGetByUserIdAsync(UserId);
                if (cart == null)
                {
                    await cartsDbRepository.CreateNewRepositoryAsync(UserId);
                }
                await cartsDbRepository.AddItemAsync(productId.Value, UserId);

                return RedirectToAction("ShowItems");
            }
            return View();
        }
        public async Task<IActionResult> ShowItemsAsync()
        {
            var currentCartItems = await cartsDbRepository?.TryGetAllItemsAsync(UserId);
            var itemsInCart = currentCartItems?.Select(x => mapper.Map<CartItemViewModel>(x)).ToList();
            ViewBag.totalCost = (await cartsDbRepository.GetTotalPriceAsync(UserId)).ToString("0.##");
            var cart = await cartsDbRepository.TryGetByUserIdAsync(UserId);
            return cart == null ? View("EmptyBag") : View("Index", itemsInCart);
        }

        public async Task<IActionResult> IncrementCountAsync(Guid productId)
        {
            if ((await cartsDbRepository.TryGetAllItemsAsync(UserId)).Any(x => x.Product.Id == productId))
            {
                await cartsDbRepository.AddItemAsync(productId, UserId);
            }
            return RedirectToAction("ShowItems");

        }

        public async Task<IActionResult> DecrementCountAsync(Guid productId)
        {
            if ((await cartsDbRepository.TryGetAllItemsAsync(UserId)).Any(x => x.Product.Id == productId))
            {
                await cartsDbRepository.ReduceItemCountAsync(productId, UserId);
            }
            return RedirectToAction("ShowItems");
        }

        public async Task<IActionResult> ClearCartAsync()
        {
            await cartsDbRepository.DeleteCartAsync(UserId);
            return RedirectToAction("ShowItems");
        }
    }
}
