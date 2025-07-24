using System;
using System.Collections.Generic;

namespace OnlineShop.db
{
    public interface IProductsRepository
    {
        Product TryGetProductById(Guid id);
        List<Product> GetAll();
        void Add(Product product) { }
        void Edit(Product product, Guid id) { }
        void Delete(Guid id) { }
    }
}
