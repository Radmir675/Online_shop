using OnlineShop.db.Models;
using System;
using System.Collections.Generic;

namespace OnlineShop.db
{
    public interface ICartsRepository
    {
        Cart TryGetByUserId(Guid userId);
        Cart CreateNewRepository(Guid userId);
        void DeleteCart(Guid userId);
        List<CartItem> TryGetAllItems(Guid userId);
        void AddItem(Guid productId, Guid userId);
        decimal GetTotalPrice(Guid userId);
        void ReduceItemCount(Guid productId, Guid userId);
    }
}