using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.db;
using OnlineShop.db.Interfaces;
using OnlineShopWebApp.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopWebApp.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ICartsRepository cartsDbRepository;
        private readonly IOrdersRepository ordersRepository;
        private readonly IMapper mapper;
        private Guid UserId = ConstantFields.UserId;

        public DataBaseContext DataBaseContext { get; }

        public OrderController(ICartsRepository cartsRepository, IOrdersRepository OrdersDbRepository, IMapper mapper)
        {
            this.cartsDbRepository = cartsRepository;
            ordersRepository = OrdersDbRepository;
            this.mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {

            if ((await cartsDbRepository.TryGetAllItemsAsync(UserId)).Count == 0)
            {
                return RedirectToAction("ShowItems", "Purchases");
            }
            OrderViewModel orderViewModel = new OrderViewModel();
            orderViewModel.UserId = UserId;
            return View("Index", new OrderViewModel { UserId = UserId });
        }

        [HttpPost]
        public async Task<IActionResult> BuyAsync(OrderViewModel currentOrder)
        {

            if (currentOrder.Email == currentOrder.Number)
            {
                ModelState.AddModelError("", "Password and number must not be the same");
                return View("Index", currentOrder);
            }
            if (!ModelState.IsValid)
            {
                return View("Index", currentOrder);
            }
            var orderToSave = mapper.Map<Order>(currentOrder);
            var user = await cartsDbRepository.TryGetByUserIdAsync(UserId);
            orderToSave.CartItems = user.CartItems.ToList();
            await ordersRepository.AddAsync(orderToSave);
            await cartsDbRepository.DeleteCartAsync(UserId);
            return View("Buy");
        }
    }
}
