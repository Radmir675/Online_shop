using System;

namespace OnlineShop.db
{
    public class Image
    {
        public Guid ImageId { get; set; }
        public Guid ProductId { get; set; }
        public string Link { get; set; }

    }
}