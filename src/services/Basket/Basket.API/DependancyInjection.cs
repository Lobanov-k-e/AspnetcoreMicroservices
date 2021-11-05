using Basket.API.Repositories;
using Basket.API.Services;
using Discount.grpc.Protos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Basket.API
{
    public static class DependancyInjection
    {
        public static void AddBasket(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(o => 
                    o.Configuration = configuration
                        .GetValue<string>("CacheSettings:ConnectionString"));

            services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options => 
                        options.Address = new Uri(configuration.GetValue<string>("GRPC:DiscountConnection")));

            services.AddScoped<IRepository, Repository>();
            services.AddScoped<FinalPriceService>();
        }
    }
}
