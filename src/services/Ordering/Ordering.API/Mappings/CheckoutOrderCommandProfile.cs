using AutoMapper;
using EventBus.Messages.Events;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.API.Mappings
{
    public class CheckoutOrderCommandProfile : Profile
    {
        public CheckoutOrderCommandProfile()
        {
            CreateMap<BasketCheckoutEvent, CheckoutOrderCommand>()
                .ForPath(e => e.BillingAdress.AddressLine, opt => opt.MapFrom(src => src.AddressLine))
                .ForPath(e => e.BillingAdress.Country, opt => opt.MapFrom(src => src.Country))
                .ForPath(e => e.BillingAdress.State, opt => opt.MapFrom(src => src.State))
                .ForPath(e => e.BillingAdress.ZipCode, opt => opt.MapFrom(src => src.ZipCode))
                .ForPath(e => e.BillingAdress.Country, opt => opt.MapFrom(src => src.Country))
                .ForPath(e => e.Payment.CardName, opt => opt.MapFrom(src => src.CardName))
                .ForPath(e => e.Payment.CardNumber, opt => opt.MapFrom(src => src.CardNumber))
                .ForPath(e => e.Payment.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod));
        }
    }
}
