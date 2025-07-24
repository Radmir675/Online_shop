using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineShop.db.Models
{
    public class CartViewModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public List<CartItem> CartItems { get; set; }
        public int Quantity { get => CartItems.Sum(x => x.Quantity); }
    }
}
