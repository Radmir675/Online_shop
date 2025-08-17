using OnlineShop.db.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineShop.db.Interfaces
{
    public interface ICartsRepository
    {
        Task<Cart> TryGetByUserIdAsync(Guid userId);
        Task<Cart> CreateNewRepositoryAsync(Guid userId);
        Task DeleteCartAsync(Guid userId);
        Task<List<CartItem>> TryGetAllItemsAsync(Guid userId);
        Task AddItemAsync(Guid productId, Guid userId);
        Task<decimal> GetTotalPriceAsync(Guid userId);
        Task ReduceItemCountAsync(Guid productId, Guid userId);
    }
}