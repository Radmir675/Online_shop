using Microsoft.EntityFrameworkCore;
using OnlineShop.db.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.db.Repositories
{
    public class OrdersDbRepository : IOrdersRepository
    {
        private readonly DataBaseContext dataBaseContext;

        public OrdersDbRepository(DataBaseContext dataBaseContext)
        {
            this.dataBaseContext = dataBaseContext;
        }

        public async Task AddAsync(Order newOrder)
        {
            await dataBaseContext.Orders.AddAsync(newOrder);
        }
        public async Task<List<Order>> GetAllAsync(Guid userId)
        {
            return await dataBaseContext.Orders
                .Include(x => x.CartItems)
                .ThenInclude(x => x.Product).ToListAsync();
        }
        public async Task<Order> TryGetByIdAsync(Guid orderId)
        {
            var order = await dataBaseContext.Orders.FirstOrDefaultAsync(x => x.OrderId == orderId);
            return order;
        }
        public async Task ChangeStatusAsync(Order order)
        {
            var currentOrder = dataBaseContext.Orders.SingleOrDefault(x => x.OrderId == order.OrderId);
            currentOrder.OrderStatus = order.OrderStatus;
            await dataBaseContext.SaveChangesAsync();
        }

    }
}
