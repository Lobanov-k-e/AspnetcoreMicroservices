using Discount.api.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Discount.api
{
    internal static class DependancyInjection
    {
        public static void AddDiscount(this IServiceCollection services)
        {
            services.AddScoped<IRepository, Repository>();
        }
    }
}
