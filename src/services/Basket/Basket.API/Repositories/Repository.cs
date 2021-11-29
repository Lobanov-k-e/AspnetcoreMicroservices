using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;
using System.Text.Json;

namespace Basket.API.Repositories
{
    public class Repository : IRepository
    {
        private readonly IDistributedCache _cache;

        public Repository(IDistributedCache cache)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }       

        public async Task<ShoppingCart> GetCart(string userName)
        {
            var basket = await _cache.GetStringAsync(userName);
            if (string.IsNullOrEmpty(basket))
                return null;

            return JsonSerializer.Deserialize<ShoppingCart>(basket);
        }

        public async Task RemoveCart(string userName)
        {
            await _cache.RemoveAsync(userName);
        }

        public async Task<ShoppingCart> UpdateOrCreateCart(ShoppingCart cart)
        {
            _ = cart ?? throw new ArgumentNullException(nameof(cart));

            await _cache.SetStringAsync(cart.UserName, JsonSerializer.Serialize(cart));

            return await GetCart(cart.UserName);
        }
    }
}
