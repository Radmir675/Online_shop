using OnlineShop.db;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace OnlineShopWebApp.Models
{
    public class OrderViewModel
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Please enter your email address")]
        [EmailAddress(ErrorMessage = "Please enter correct email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your full name")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Your full name is very short")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Please enter your phone number")]
        [StringLength(15, MinimumLength = 11, ErrorMessage = "Your phone number is very short")]
        [RegularExpression(@"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$", ErrorMessage = "Format wrong")]
        public string Number { get; set; }
        public List<CartItem> CartItems { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public decimal Cost
        {
            get
            {
                return CartItems?.Sum(x => x.Quantity * x.Product.Cost) ?? 0;
            }
        }
        public OrderStatusViewModel OrderStatus { get; set; }
    }
}
