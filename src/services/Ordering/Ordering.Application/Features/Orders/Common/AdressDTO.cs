using AutoMapper;
using Ordering.Application.Mappings;
using Ordering.Domain.ValueObjects;

namespace Ordering.Application.Features.Orders.Common
{
    public class AdressDTO : IMapFrom<Adress>
    {
        public string AddressLine { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

        public void Mapping(Profile profile) => profile.CreateMap(typeof(Adress), GetType()).ReverseMap();

    }
}