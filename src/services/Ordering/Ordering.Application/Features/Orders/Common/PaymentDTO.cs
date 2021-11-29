using AutoMapper;
using Ordering.Application.Mappings;
using Ordering.Domain.ValueObjects;

namespace Ordering.Application.Features.Orders.Common
{
    public class PaymentDTO : IMapFrom<Payment>
    {
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string Expiration { get; set; }
        public string CVV { get; set; }
        public int PaymentMethod { get; set; }

        public void Mapping(Profile profile) => profile.CreateMap(typeof(Payment), GetType()).ReverseMap();
    }
}