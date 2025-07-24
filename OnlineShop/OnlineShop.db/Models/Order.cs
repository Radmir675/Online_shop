using System;
using System.Collections.Generic;

namespace OnlineShop.db
{
    public class Order
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Number { get; set; }
        public List<CartItem> CartItems { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public OrderStatus OrderStatus { get; set; }

        public Order()
        {
            Date = DateTime.Today.ToShortDateString();
            Time = DateTime.Now.ToLongTimeString();
        }
    }
}
