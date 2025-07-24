using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OnlineShop.db;
using OnlineShopWebApp.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineShopWebApp.Helpers
{
    public class ProductCache : BackgroundService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IMemoryCache memoryCache;


        public ProductCache(IServiceProvider serviceProvider, IMemoryCache memoryCache)
        {
            this.serviceProvider = serviceProvider;
            this.memoryCache = memoryCache;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                CachingAllProducts();
                await Task.Delay(60000, stoppingToken);
            }
        }

        private void CachingAllProducts()
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var dataBaseContext = scope.ServiceProvider.GetRequiredService<DataBaseContext>();
                var products = dataBaseContext?.Products.Include(x => x.Images).ToList();
                if (products != null)
                {
                    memoryCache.Set(ConstantFields.KeyCacheAllProducts, products);
                }
                foreach (var product in products)
                {
                    if (product != null)
                    {
                        memoryCache.Set(product.Id, product);
                    }
                }
            }
        }
    }
}
