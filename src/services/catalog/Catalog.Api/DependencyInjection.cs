using Catalog.Api.Persistence;
using Catalog.Api.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Api
{
    public static class DependencyInjection
    {
        public static void AddCatalog(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<ICatalogContext, CatalogContext>();
            serviceCollection.AddTransient<IProductRepository, ProductRepository>();
        }
    }
}
