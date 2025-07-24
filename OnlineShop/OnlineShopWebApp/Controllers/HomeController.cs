using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using OnlineShop.db;
using OnlineShopWebApp.Helpers;
using OnlineShopWebApp.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace OnlineShopWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMapper mapper;
        private readonly IMemoryCache memoryCache;

        public HomeController(IMapper mapper, IMemoryCache memoryCache)
        {
            this.mapper = mapper;
            this.memoryCache = memoryCache;
        }

        public IActionResult Index(SearchViewModel searchViewModel, int page = 1)
        {
            memoryCache.TryGetValue<List<Product>>(ConstantFields.KeyCacheAllProducts, out var allProducts);

            //фильтрация
            var resultProducts = FilterByParameters(searchViewModel, allProducts);
            //сортировка
            resultProducts = SortByParameters(searchViewModel, resultProducts);
            //пагинация
            int pageSize = 3; // количество элементов на странице
            var items = resultProducts.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            PageViewModel pageViewModel = new PageViewModel(resultProducts.Count, page, pageSize);

            var productsToView = items?.Select(product => mapper.Map<ProductViewModel>(product)).ToList() ?? new List<ProductViewModel>();

            // формируем модель представления
            HomePageViewModel homePageViewModel = new HomePageViewModel()
            {
                ProductViewModel = productsToView,
                SearchViewModel = searchViewModel,
                PageViewModel = pageViewModel
            };

            return View(homePageViewModel);
        }

        private List<Product> FilterByParameters(SearchViewModel searchViewModel, List<Product> products)
        {
            if (searchViewModel.DownCost.HasValue)
            {
                products = products.Where(x => x.Cost >= searchViewModel.DownCost).OrderBy(x => x.Cost).ToList();
            }
            if (searchViewModel.UpCost.HasValue)
            {
                products = products.Where(x => x.Cost <= searchViewModel.UpCost).OrderBy(x => x.Cost).ToList();
            }
            if (searchViewModel.KeyWord != null)
            {
                products = products.Where(x => x.Name.ToLower().Contains(searchViewModel.KeyWord.ToLower())).OrderBy(x => x.Cost).ToList();
            }
            return products;
        }
        private List<Product> SortByParameters(SearchViewModel searchViewModel, List<Product> products)
        {
            switch (searchViewModel.SortProductStatus)
            {
                case SortProductsStatus.Ascend:
                    return products.OrderBy(x => x.Cost).ToList();
                case SortProductsStatus.Descend:
                    return products.OrderByDescending(x => x.Cost).ToList();
            }
            return products;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
