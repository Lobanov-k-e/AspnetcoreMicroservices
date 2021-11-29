using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;
using Ordering.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Persisence
{
    public class SeedOrderData
    {
        public static async Task Seed(OrderContext context, ILogger<SeedOrderData> logger)
        {

            if (context.Order.Count() != 0 )
                return;

            context.Order.AddRange(Enumerable.Range(0, 4).Select(i => GetPreconfiguredOrder()));
            await context.SaveChangesAsync();
            logger.LogInformation($"Database seed completed. context: {typeof(OrderContext)}");
            
        }

        private static Order GetPreconfiguredOrder()
        {
            var adress = new Adress
            {
                AddressLine = "city",
                Country = "nowhere"
            };

            var order = new Order()
            {
                UserName = "swn",
                FirstName = "Test",
                LastName = "Test",
                EmailAddress = "test@example.com",
                BillingAdress = adress
            };

            return order;
        }
    }
}
