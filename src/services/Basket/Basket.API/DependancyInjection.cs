using Basket.API.Repositories;
using Basket.API.Services;
using Discount.grpc.Protos;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace Basket.API
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddBasket(this IServiceCollection services, IConfiguration configuration)
        {
            AddRedisStackExchange(services, configuration);
            AddGrpc(services, configuration);
            AddEventbus(services, configuration);

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddScoped<IRepository, Repository>();
            services.AddScoped<FinalPriceService>();          

            return services;
        }

        private static void AddRedisStackExchange(IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(o =>
                    o.Configuration = configuration
                        .GetValue<string>("CacheSettings:ConnectionString"));
        }

        private static void AddGrpc(IServiceCollection services, IConfiguration configuration)
        {
            services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
                                    options.Address = new Uri(configuration.GetValue<string>("GRPC:DiscountConnection")));
        }

        private static void AddEventbus(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(conf =>
            {
                conf.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(configuration.GetValue<string>("EventBus:Host"));

                });
            });

            services.AddMassTransitHostedService();
        }
    }
}
