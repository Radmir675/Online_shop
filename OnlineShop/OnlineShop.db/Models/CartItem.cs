using OnlineShop.db.Models;
using System;

namespace OnlineShop.db
{
    public class CartItem
    {
        public Guid Id { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; } = 0;
        public Cart Cart { get; set; }

        public Order Order { get; set; }

        public CartItem()
        {

        }
        public CartItem(Product product)
        {
            Product = product;
            Quantity++;
        }
        public CartItem(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }
    }
}


