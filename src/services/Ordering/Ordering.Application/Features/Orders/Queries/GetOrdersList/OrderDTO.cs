using Ordering.Application.Features.Orders.Common;
using Ordering.Application.Mappings;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList
{
    public class OrderDTO : IMapFrom<Order>
    {
        public string UserName { get; set; }
        public decimal TotalPrice { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }

        public AdressDTO BillingAdress { get; set; }
        public PaymentDTO Payment { get; set; }
    }
}