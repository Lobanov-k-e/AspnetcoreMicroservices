using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API.Entities
{
    public class ShoppingCart
    {
        public string UserName { get; set; }
        public IEnumerable<ShoppingCartItem> Items { get; set; }

        public ShoppingCart(string userName)
        {
            UserName = userName;
            Items = new List<ShoppingCartItem>();
        }

        public ShoppingCart() : this(string.Empty)
        {

        }

        public decimal TotalPrice => Items.Sum(i => i.GetTotal());

        public static ShoppingCart Empty(string userName) => new ShoppingCart(userName);           
        
    }
}
