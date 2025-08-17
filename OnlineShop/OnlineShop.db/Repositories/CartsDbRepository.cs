using Microsoft.EntityFrameworkCore;
using OnlineShop.db.Interfaces;
using OnlineShop.db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<Cart> TryGetByUserIdAsync(Guid userId)
        {
            return await dataBaseContext.Carts
                                            .Include(x => x.CartItems)
                                            .ThenInclude(x => x.Product)
                                             .FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<Cart> CreateNewRepositoryAsync(Guid userId)
        {
            dataBaseContext.Carts.Add(new Cart(userId));
            await dataBaseContext.SaveChangesAsync();
            return await TryGetByUserIdAsync(userId);
        }

        public async Task AddItemAsync(Guid productId, Guid userId)
        {

            if (dataBaseContext.Products.Any(x => x.Id == productId))
            {
                var cart = await TryGetByUserIdAsync(userId);
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
                        await CreateNewRepositoryAsync(userId);
                    }
                    cart.CartItems.Add(itemToAdd);
                }
                await dataBaseContext.SaveChangesAsync();
            }
        }
        public async Task ReduceItemCountAsync(Guid productId, Guid userId)
        {
            var user = await TryGetByUserIdAsync(userId);
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
                    await DeleteCartAsync(userId);
                }
            }
            await dataBaseContext.SaveChangesAsync();
        }

        public async Task<List<CartItem>> TryGetAllItemsAsync(Guid userId)
        {
            var user = await TryGetByUserIdAsync(userId);
            return user?.CartItems.ToList() ?? new List<CartItem>();
        }

        public async Task<decimal> GetTotalPriceAsync(Guid userId)
        {
            var user = await TryGetByUserIdAsync(userId);
            return user?.CartItems.Sum(x => x.Quantity * x.Product.Cost) ?? 0;
        }
        public async Task DeleteCartAsync(Guid userId)
        {
            var cart = await TryGetByUserIdAsync(userId);

            if (cart == null)
            {
                return;
            }
            dataBaseContext.Carts.Remove(cart);
            await dataBaseContext.SaveChangesAsync();
        }
    }
}
