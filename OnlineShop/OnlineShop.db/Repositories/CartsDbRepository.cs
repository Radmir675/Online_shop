using Microsoft.EntityFrameworkCore;
using OnlineShop.db.Interfaces;
using OnlineShop.db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineShop.db.Repositories
{
    public class CartsDbRepository : ICartsRepository
    {
        private readonly DataBaseContext dataBaseContext;
        private readonly IProductsRepository productsDbRepository;

        public CartsDbRepository(DataBaseContext dataBaseContext, IProductsRepository productsDbRepository)
        {

            this.dataBaseContext = dataBaseContext;
            this.productsDbRepository = productsDbRepository;
        }
        public async Task<Cart> TryGetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await dataBaseContext.Carts
                                            .Include(x => x.CartItems)
                                            .ThenInclude(x => x.Product)
                                             .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
        }

        public async Task<Cart> CreateNewRepositoryAsync(Guid userId, CancellationToken cancellationToken)
        {
            dataBaseContext.Carts.Add(new Cart(userId));
            await dataBaseContext.SaveChangesAsync(cancellationToken);
            return await TryGetByUserIdAsync(userId, cancellationToken);
        }

        public async Task AddItemAsync(Guid productId, Guid userId, CancellationToken cancellationToken)
        {

            if (dataBaseContext.Products.Any(x => x.Id == productId))
            {
                var cart = await TryGetByUserIdAsync(userId, cancellationToken);
                if (cart.CartItems.Any(x => x.Product.Id == productId))
                {
                    var foundItem = cart.CartItems.Single(purchase => purchase.Product.Id == productId);
                    foundItem.Quantity++;
                }
                else
                {
                    var itemToAdd = new CartItem(await productsDbRepository.TryGetProductByIdAsync(productId));
                    cart = dataBaseContext.Carts.FirstOrDefault(x => x.UserId == userId);
                    if (cart == null)
                    {
                        await CreateNewRepositoryAsync(userId, cancellationToken);
                    }
                    cart.CartItems.Add(itemToAdd);
                }
                await dataBaseContext.SaveChangesAsync(cancellationToken);
            }
        }
        public async Task ReduceItemCountAsync(Guid productId, Guid userId, CancellationToken cancellationToken)
        {
            var user = await TryGetByUserIdAsync(userId, cancellationToken);
            var existingOrder = user?.CartItems;
            if (existingOrder == null) return;
            var cartItem = existingOrder?.SingleOrDefault(x => x.Product.Id == productId);
            if (cartItem.Quantity > 1)
            {
                cartItem.Quantity--;
            }
            else
            {
                existingOrder.Remove(cartItem);
                if (!existingOrder.Any())
                {
                    await DeleteCartAsync(userId, cancellationToken);
                }
            }
            await dataBaseContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<CartItem>> TryGetAllItemsAsync(Guid userId, CancellationToken cancellationToken)
        {
            var user = await TryGetByUserIdAsync(userId, cancellationToken);
            return user?.CartItems.ToList() ?? new List<CartItem>();
        }

        public async Task<decimal> GetTotalPriceAsync(Guid userId, CancellationToken cancellationToken)
        {
            var user = await TryGetByUserIdAsync(userId, cancellationToken);
            return user?.CartItems.Sum(x => x.Quantity * x.Product.Cost) ?? 0;
        }
        public async Task DeleteCartAsync(Guid userId, CancellationToken cancellationToken)
        {
            var cart = await TryGetByUserIdAsync(userId, cancellationToken);

            if (cart == null)
            {
                return;
            }
            dataBaseContext.Carts.Remove(cart);
            await dataBaseContext.SaveChangesAsync(cancellationToken);
        }
    }
}
