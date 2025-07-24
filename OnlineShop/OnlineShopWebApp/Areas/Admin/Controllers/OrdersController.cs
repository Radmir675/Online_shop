using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.db;
using OnlineShopWebApp.Models;
using System;
using System.Linq;

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
        public IActionResult ShowOrders()
        {
            var orders = ordersDbRepository.GetAll(UserId);
            var ordersToView = orders.Select(order => mapper.Map<OrderViewModel>(order)).ToList();
            return View("Orders", ordersToView);
        }
        [HttpPost]
        public IActionResult ShowOrderInfo(Guid orderId)
        {
            var order = ordersDbRepository?.TryGetById(orderId);
            if (order == null)
            {
                return RedirectToAction("ShowOrders");
            }
            var orderToView = mapper.Map<OrderViewModel>(order);
            return View("OrderInfo", orderToView);
        }

        [HttpPost]
        public IActionResult SaveStatus(Guid orderId, OrderStatusViewModel newStatus)
        {
            var order = ordersDbRepository?.TryGetById(orderId);
            if (order != null)
            {
                var status = mapper.Map<OrderStatus>(newStatus);
                order.OrderStatus = status;
                ordersDbRepository.ChangeStatus(order);
            }
            return RedirectToAction("ShowOrders");
        }
    }
}
