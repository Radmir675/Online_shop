using OnlineShopWebApp.Helpers;

namespace OnlineShopWebApp.Models
{
    public class SearchViewModel
    {

        public decimal? DownCost { get; set; }
        public decimal? UpCost { get; set; }
        public string KeyWord { get; set; }

        public SortProductsStatus SortProductStatus { get; set; }
    }
}
