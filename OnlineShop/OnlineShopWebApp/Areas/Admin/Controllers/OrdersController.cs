using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.db;
using OnlineShop.db.Interfaces;
using OnlineShopWebApp.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopWebApp.Areas.Admin.Controllers
{
    [Area(ConstantFields.AdminRoleName)]
    [Authorize(Roles = ConstantFields.AdminRoleName)]
    public class OrdersController : Controller
    {
        private readonly IOrdersRepository ordersDbRepository;
        private readonly IMapper mapper;
        private Guid UserId = ConstantFields.UserId;
        public OrdersController(IOrdersRepository ordersDbRepository, IMapper mapper)
        {
            this.ordersDbRepository = ordersDbRepository;
            this.mapper = mapper;
        }
        public async Task<IActionResult> ShowOrders()
        {
            var orders = await ordersDbRepository.GetAllAsync(UserId);
            var ordersToView = orders.Select(order => mapper.Map<OrderViewModel>(order)).ToList();
            return View("Orders", ordersToView);
        }
        [HttpPost]
        public async Task<IActionResult> ShowOrderInfo(Guid orderId)
        {
            var order = await ordersDbRepository?.TryGetByIdAsync(orderId);
            if (order == null)
            {
                return RedirectToAction("ShowOrders");
            }
            var orderToView = mapper.Map<OrderViewModel>(order);
            return View("OrderInfo", orderToView);
        }

        [HttpPost]
        public async Task<IActionResult> SaveStatus(Guid orderId, OrderStatusViewModel newStatus)
        {
            var order = await ordersDbRepository?.TryGetByIdAsync(orderId);
            if (order != null)
            {
                var status = mapper.Map<OrderStatus>(newStatus);
                order.OrderStatus = status;
                ordersDbRepository.ChangeStatusAsync(order);
            }
            return RedirectToAction("ShowOrders");
        }
    }
}
