using Basket.API.Entities;
using System.Threading.Tasks;

namespace Basket.API.Repositories
{
    public interface IRepository
    {
        Task<ShoppingCart> GetCart(string userName);
        Task<ShoppingCart> UpdateOrCreateCart(ShoppingCart cart);
        Task RemoveCart(string userName);
    }
}
