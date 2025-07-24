using Microsoft.EntityFrameworkCore;
using OnlineShop.db;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineShopWebApp.db
{
    public class ProductsDbRepository : IProductsRepository
    {
        private readonly DataBaseContext dataBaseContext;

        public ProductsDbRepository(DataBaseContext dataBaseContext)
        {
            this.dataBaseContext = dataBaseContext;
        }
        public List<Product> GetAll()
        {
            var products = dataBaseContext?.Products.Include(x => x.Images).ToList();
            return products;
        }

        public Product TryGetProductById(Guid id)
        {
            return dataBaseContext.Products.Include(x => x.Images).FirstOrDefault(product => product.Id == id);
        }
        public void Add(Product product)
        {
            dataBaseContext.Products.Add(product);
            dataBaseContext.SaveChanges();
        }
        public void Edit(Product product, Guid id)
        {
            var currentProduct = TryGetProductById(id);
            currentProduct.Name = product.Name;
            currentProduct.Cost = product.Cost;
            currentProduct.ShortDescription = product.ShortDescription;
            currentProduct.Images = product.Images;
            dataBaseContext.SaveChanges();
        }
        public void Delete(Guid id)
        {
            var productToDelete = TryGetProductById(id);
            if (productToDelete != null)
            {
                dataBaseContext.Remove(productToDelete);
                dataBaseContext.SaveChanges();
            }
        }
    }
}
