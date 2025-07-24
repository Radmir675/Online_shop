using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.db;
using OnlineShop.db.Models;
using OnlineShopWebApp.Models;
using System;
using System.Linq;

namespace OnlineShopWebApp.Controllers
{
    [Authorize]
    public class PurchasesController : Controller
    {
        private readonly ICartsRepository cartsDbRepository;
        private readonly IProductsRepository productsDbRepository;
        private readonly IMapper mapper;
        private Guid UserId = ConstantFields.UserId;
        Cart cart;
        public PurchasesController(ICartsRepository cartsDbRepository, IProductsRepository productsRepository, IMapper mapper)
        {
            this.cartsDbRepository = cartsDbRepository;
            this.productsDbRepository = productsRepository;
            this.mapper = mapper;
            cart = cartsDbRepository.TryGetByUserId(UserId);
        }

        public IActionResult Index(Guid? productId)
        {
            if (productId.HasValue)
            {
                cart = cartsDbRepository.TryGetByUserId(UserId);
                if (cart == null)
                {
                    cart = cartsDbRepository.CreateNewRepository(UserId);
                }
                cartsDbRepository.AddItem(productId.Value, UserId);

                return RedirectToAction("ShowItems");
            }
            else
            {
                return View();
            }

        }
        public IActionResult ShowItems()
        {
            var currentCartItems = cartsDbRepository?.TryGetAllItems(UserId);
            var itemsInCart = currentCartItems?.Select(x => mapper.Map<CartItemViewModel>(x)).ToList();
            ViewBag.totalCost = cartsDbRepository.GetTotalPrice(UserId).ToString("0.##");
            return cart == null ? View("EmptyBag") : View("Index", itemsInCart);
        }

        public IActionResult IncrementCount(Guid productId)
        {
            if (cartsDbRepository.TryGetAllItems(UserId).Any(x => x.Product.Id == productId))
            {
                cartsDbRepository.AddItem(productId, UserId);

            }
            return RedirectToAction("ShowItems");

        }

        public IActionResult DecrementCount(Guid productId)
        {
            if (cartsDbRepository.TryGetAllItems(UserId).Any(x => x.Product.Id == productId))
            {
                cartsDbRepository.ReduceItemCount(productId, UserId);
            }
            return RedirectToAction("ShowItems");
        }

        public IActionResult ClearCart()
        {
            cartsDbRepository.DeleteCart(UserId);

            return RedirectToAction("ShowItems");
        }
    }
}
