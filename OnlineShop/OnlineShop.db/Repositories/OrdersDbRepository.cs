using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineShop.db
{
    public class OrdersDbRepository : IOrdersRepository
    {
        private readonly DataBaseContext dataBaseContext;

        public OrdersDbRepository(DataBaseContext dataBaseContext)
        {
            this.dataBaseContext = dataBaseContext;
        }

        public void Add(Order newOrder)
        {
            dataBaseContext.Orders.Add(newOrder);
        }
        public List<Order> GetAll(Guid userId)
        {
            return dataBaseContext.Orders.Include(x => x.CartItems).ThenInclude(x => x.Product).ToList();
        }
        public Order TryGetById(Guid orderId)
        {
            var order = dataBaseContext.Orders.FirstOrDefault(x => x.OrderId == orderId);
            return order;
        }
        public void ChangeStatus(Order order)
        {
            var currentOrder = dataBaseContext.Orders.SingleOrDefault(x => x.OrderId == order.OrderId);
            currentOrder.OrderStatus = order.OrderStatus;
            dataBaseContext.SaveChanges();
        }

    }
}
