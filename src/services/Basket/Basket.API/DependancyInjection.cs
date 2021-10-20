using Basket.API.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Basket.API
{
    public static class DependancyInjection
    {
        public static void AddBasket(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(o => o.Configuration = configuration.GetValue<string>("CacheSettings:ConnectionString"));
            services.AddScoped<IRepository, Repository>();
        }
    }
}
