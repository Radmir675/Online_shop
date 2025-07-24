using System;
using System.Collections.Generic;

namespace OnlineShop.db.Models
{
    public class Cart
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public List<CartItem> CartItems { get; set; }
        public Cart()
        {

        }
        public Cart(Guid userId)
        {
            UserId = userId;
            CartItems = new List<CartItem>();
        }
    }
}
