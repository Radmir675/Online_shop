using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.db;
using OnlineShopWebApp.Models;
using System;
using System.Linq;

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
        public IActionResult Index()
        {

            if (cartsDbRepository.TryGetAllItems(UserId).Count == 0)
            {
                return RedirectToAction("ShowItems", "Purchases");
            }
            OrderViewModel orderViewModel = new OrderViewModel();
            orderViewModel.UserId = UserId;
            return View("Index", new OrderViewModel { UserId = UserId });
        }

        [HttpPost]
        public IActionResult Buy(OrderViewModel currentOrder)
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
            orderToSave.CartItems = cartsDbRepository.TryGetByUserId(UserId).CartItems.ToList();
            ordersRepository.Add(orderToSave);
            cartsDbRepository.DeleteCart(UserId);
            return View("Buy");
        }
    }
}
