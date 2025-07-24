namespace OnlineShopWebApp.Models
{
    public class CartItemViewModel
    {
        public ProductViewModel Product { get; set; }
        public int Quantity { get; set; }
        public decimal Cost { get => Product.Cost * Quantity; }
    }
}