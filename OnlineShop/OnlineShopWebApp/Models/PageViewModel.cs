using System;

namespace OnlineShopWebApp.Models
{

    public class PageViewModel
    {
        public int PageNumber { get; private set; }
        public int TotalPages { get; private set; }

        public PageViewModel(int itemsCount, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(itemsCount / (double)pageSize);
        }
        public bool HasPreviousPage => PageNumber > 0;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}
