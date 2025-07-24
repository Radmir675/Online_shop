using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using OnlineShop.db;
using System;

namespace OnlineShopWebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IMapper mapper;
        private readonly IMemoryCache memoryCache;

        public ProductController(IMapper mapper, IMemoryCache memoryCache)
        {
            this.mapper = mapper;
            this.memoryCache = memoryCache;
        }
        public IActionResult Index(Guid id)
        {
            memoryCache.TryGetValue<Product>(id, out var product);
            var productViewModel = mapper.Map<ProductViewModel>(product);
            return product == null ? View("Error") : View(productViewModel);
        }
    }
}
