using Microsoft.EntityFrameworkCore;
using OnlineShop.db.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineShop.db
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
        public Cart TryGetByUserId(Guid userId)
        {
            return dataBaseContext.Carts
                                            .Include(x => x.CartItems)
                                            .ThenInclude(x => x.Product)
                                             .FirstOrDefault(x => x.UserId == userId);
        }

        public Cart CreateNewRepository(Guid userId)
        {
            dataBaseContext.Carts.Add(new Cart(userId));
            dataBaseContext.SaveChanges();
            return TryGetByUserId(userId);
        }

        public void AddItem(Guid productId, Guid userId)
        {
            
            if (dataBaseContext.Products.Any(x => x.Id == productId))
            {
                var cart = TryGetByUserId(userId);
                if (cart.CartItems.Any(x => x.Product.Id == productId))
                {
                    var foundItem = cart.CartItems.Single(purchase => purchase.Product.Id == productId);
                    foundItem.Quantity++;
                }
                else
                {
                    var itemToAdd = new CartItem(productsDbRepository.TryGetProductById(productId));
                    cart = dataBaseContext.Carts.FirstOrDefault(x => x.UserId == userId);
                    if (cart == null)
                    {
                        CreateNewRepository(userId);
                    }
                    cart.CartItems.Add(itemToAdd);
                }
                dataBaseContext.SaveChanges();
            }
        }
        public void ReduceItemCount(Guid productId, Guid userId)
        {
            var existingOrder = TryGetByUserId(userId).CartItems;
            var cartItem = existingOrder.SingleOrDefault(x => x.Product.Id == productId);
            if (cartItem.Quantity > 1)
            {
                cartItem.Quantity--;
            }
            else
            {
                existingOrder.Remove(cartItem);
                if (existingOrder.Count() == 0)
                {
                    DeleteCart(userId);
                }
            }
            dataBaseContext.SaveChanges();
        }

        public List<CartItem> TryGetAllItems(Guid userId)
        {
            return TryGetByUserId(userId)?.CartItems.ToList() ?? new List<CartItem>();
        }

        public decimal GetTotalPrice(Guid userId)
        {
            decimal totalPrice = 0;
            return totalPrice = TryGetByUserId(userId)?.CartItems.Sum(x => x.Quantity * x.Product.Cost) ?? 0;
        }
        public void DeleteCart(Guid userId)
        {
            var cart = TryGetByUserId(userId);
            
            if (cart == null)
            {
                return;
            }
            dataBaseContext.Carts.Remove(cart);
            dataBaseContext.SaveChanges();

        }

    }
}
