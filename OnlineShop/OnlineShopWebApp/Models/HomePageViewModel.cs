using System.Collections.Generic;

namespace OnlineShopWebApp.Models
{
    public class HomePageViewModel
    {
        public SearchViewModel SearchViewModel { get; set; }
        public List<ProductViewModel> ProductViewModel { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
