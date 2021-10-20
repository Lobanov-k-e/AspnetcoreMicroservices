using Catalog.Api.Entities;
using Catalog.Api.Persistence;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Api.Repositories
{
    internal class ProductRepository : IProductRepository
    {        
        private readonly IMongoCollection<Product> _products;

        public ProductRepository(ICatalogContext context)
        {
            _ = context ?? throw new ArgumentNullException(nameof(context));

            _products = context.Products;
        }

        public async Task AddAsync(Product product)
        {
            await _products.InsertOneAsync(product);            
        }

        public async Task<bool> UpdateAsync(Product product)
        {          
            var result = await _products
                .ReplaceOneAsync(p => p.Id.Equals(product.Id), product);

            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task DeleteAsync(string id)
        {
            await _products.DeleteOneAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category)
        {
            return await _products
               .Find(p => p.Category == category)
               .ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(string id)
        {
            return await _products
                .Find(p => p.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Product> GetProductByNameAsync(string name)
        {
            return await _products
               .Find(p => p.Name == name)
               .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _products
                .Find(_ => true)
                .ToListAsync();   
        }      
    }
}
