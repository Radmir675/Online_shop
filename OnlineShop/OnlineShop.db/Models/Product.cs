using System;
using System.Collections.Generic;

namespace OnlineShop.db
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public string ShortDescription { get; set; }
        public List<CartItem> CartItems { get; set; }
        public List<Image> Images { get; set; }

        public Product()
        {

        }
        public Product(string name, decimal cost, string shortDescription, List<Image> images)
        {
            Name = name;
            Cost = cost;
            ShortDescription = shortDescription;
            Images = images;
        }
    }
}
