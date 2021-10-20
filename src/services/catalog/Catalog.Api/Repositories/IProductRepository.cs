using Catalog.Api.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Api.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<Product> GetProductByIdAsync(string id);
        Task<Product> GetProductByNameAsync(string name);
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category);

        Task AddAsync(Product product);
        Task<bool> UpdateAsync(Product product);

        Task DeleteAsync(string id);


    }
}
