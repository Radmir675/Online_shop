using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineShop.db.Interfaces
{
    public interface IProductsRepository
    {
        Task<Product> TryGetProductByIdAsync(Guid id);
        Task<List<Product>> GetAllAsync();
        Task AddAsync(Product product);
        Task EditAsync(Product product, Guid id);
        Task DeleteAsync(Guid id);
    }
}
