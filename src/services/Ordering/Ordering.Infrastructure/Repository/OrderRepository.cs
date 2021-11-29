using Microsoft.EntityFrameworkCore;
using Ordering.Application.Contracts.Persisence;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Persisence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Repository
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(OrderContext context) 
            : base(context)
        {
        }

        public async Task<IReadOnlyList<Order>> GetOrdersByUserName(string userName)
        {
            var orders = await _set.ToListAsync();
            return await _set
                .Where(o => o.UserName == userName)
                .ToListAsync();            
        }
    }
}
