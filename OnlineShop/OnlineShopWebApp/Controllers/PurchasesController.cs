using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.db.Interfaces;
using OnlineShopWebApp.Models;
using System;
using System.Linq;
using System.Threading;
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

        public async Task<IActionResult> Index(Guid? productId, CancellationToken cancellationToken = default)
        {
            if (productId.HasValue)
            {
                var cart = await cartsDbRepository.TryGetByUserIdAsync(UserId, cancellationToken);
                if (cart == null)
                {
                    await cartsDbRepository.CreateNewRepositoryAsync(UserId, cancellationToken);
                }
                await cartsDbRepository.AddItemAsync(productId.Value, UserId, cancellationToken);

                return RedirectToAction("ShowItems");
            }
            return View();
        }
        public async Task<IActionResult> ShowItemsAsync(CancellationToken cancellationToken)
        {
            var currentCartItems = await cartsDbRepository?.TryGetAllItemsAsync(UserId, cancellationToken);
            var itemsInCart = currentCartItems?.Select(x => mapper.Map<CartItemViewModel>(x)).ToList();
            ViewBag.totalCost = (await cartsDbRepository.GetTotalPriceAsync(UserId, cancellationToken)).ToString("0.##");
            var cart = await cartsDbRepository.TryGetByUserIdAsync(UserId, cancellationToken);
            return cart == null ? View("EmptyBag") : View("Index", itemsInCart);
        }

        public async Task<IActionResult> IncrementCountAsync(Guid productId, CancellationToken cancellationToken)
        {
            if ((await cartsDbRepository.TryGetAllItemsAsync(UserId, cancellationToken)).Any(x => x.Product.Id == productId))
            {
                await cartsDbRepository.AddItemAsync(productId, UserId, cancellationToken);
            }
            return RedirectToAction("ShowItems");

        }

        public async Task<IActionResult> DecrementCountAsync(Guid productId, CancellationToken cancellationToken)
        {
            if ((await cartsDbRepository.TryGetAllItemsAsync(UserId, cancellationToken)).Any(x => x.Product.Id == productId))
            {
                await cartsDbRepository.ReduceItemCountAsync(productId, UserId, cancellationToken);
            }
            return RedirectToAction("ShowItems");
        }

        public async Task<IActionResult> ClearCartAsync(CancellationToken cancellationToken)
        {
            await cartsDbRepository.DeleteCartAsync(UserId, cancellationToken);
            return RedirectToAction("ShowItems");
        }
    }
}
