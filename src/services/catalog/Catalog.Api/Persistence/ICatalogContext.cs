using Catalog.Api.Entities;
using MongoDB.Driver;

namespace Catalog.Api.Persistence
{
    interface ICatalogContext
    {
        IMongoCollection<Product> Products { get;}
    }
}
