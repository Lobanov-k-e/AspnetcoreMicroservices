using Catalog.Api.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;

namespace Catalog.Api.Persistence
{
    public class CatalogContext : ICatalogContext
    {
        public CatalogContext(IConfiguration configuration)
        {
            string connectionString = configuration.GetValue<string>("DatabaseSettings:ConnectionString");
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(configuration["DatabaseSettings:DatabaseName"]);
            Products = database.GetCollection<Product>(configuration["DatabaseSettings:CollectionName"]);
            
        }

        public IMongoCollection<Product> Products { get; }
    }
}
