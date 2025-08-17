using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineShop.db.Interfaces
{
    public interface IOrdersRepository
    {
        Task AddAsync(Order newOrder);
        Task<List<Order>> GetAllAsync(Guid userId);
        Task<Order> TryGetByIdAsync(Guid orderId);
        Task ChangeStatusAsync(Order order);
    }
}
