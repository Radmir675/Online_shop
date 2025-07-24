using System;
using System.Collections.Generic;

namespace OnlineShop.db
{
    public interface IOrdersRepository
    {
        void Add(Order newOrder);
        List<Order> GetAll(Guid userId);
        Order TryGetById(Guid orderId);
        void ChangeStatus(Order order);
    }
}
