using Microsoft.EntityFrameworkCore;
using OnlineShop.db.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineShop.db.Repositories
{
    public class ProductsDbRepository : IProductsRepository
    {
        private readonly DataBaseContext dataBaseContext;

        public ProductsDbRepository(DataBaseContext dataBaseContext)
        {
            this.dataBaseContext = dataBaseContext;
        }
        public async Task<List<Product>> GetAllAsync()
        {
            var products = await dataBaseContext?.Products
                .Include(x => x.Images)
                .ToListAsync();
            return products;
        }

        public async Task<Product> TryGetProductByIdAsync(Guid id)
        {
            return await dataBaseContext.Products
                .Include(x => x.Images)
                .FirstOrDefaultAsync(product => product.Id == id);
        }
        public async Task AddAsync(Product product)
        {
            dataBaseContext.Products.Add(product);
            await dataBaseContext.SaveChangesAsync();
        }
        public async Task EditAsync(Product product, Guid id)
        {
            var currentProduct = await TryGetProductByIdAsync(id);
            currentProduct.Name = product.Name;
            currentProduct.Cost = product.Cost;
            currentProduct.ShortDescription = product.ShortDescription;
            currentProduct.Images = product.Images;
            await dataBaseContext.SaveChangesAsync();
        }
        public async Task DeleteAsync(Guid id)
        {
            var productToDelete = await TryGetProductByIdAsync(id);
            if (productToDelete != null)
            {
                dataBaseContext.Remove(productToDelete);
                await dataBaseContext.SaveChangesAsync();
            }
        }
    }
}
