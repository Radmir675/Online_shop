using OnlineShop.db.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineShop.db.Interfaces
{
    public interface ICartsRepository
    {
        Task<Cart> TryGetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<Cart> CreateNewRepositoryAsync(Guid userId, CancellationToken cancellationToken);
        Task DeleteCartAsync(Guid userId, CancellationToken cancellationToken);
        Task<List<CartItem>> TryGetAllItemsAsync(Guid userId, CancellationToken cancellationToken);
        Task AddItemAsync(Guid productId, Guid userId, CancellationToken cancellationToken);
        Task<decimal> GetTotalPriceAsync(Guid userId, CancellationToken cancellationToken);
        Task ReduceItemCountAsync(Guid productId, Guid userId, CancellationToken cancellationToken);
    }
}