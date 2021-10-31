using Discount.grpc.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Discount.grpc
{
    internal static class DependancyInjection
    {
        public static void AddDiscount(this IServiceCollection services)
        {
            services.AddScoped<IRepository, Repository>();
        }
    }
}
