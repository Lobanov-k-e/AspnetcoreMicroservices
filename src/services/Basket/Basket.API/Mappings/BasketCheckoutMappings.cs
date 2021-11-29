using AutoMapper;
using Basket.API.Entities;
using EventBus.Messages.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API.Mappings
{
    public class BasketCheckoutMappings : Profile
    {
        public BasketCheckoutMappings()
        {
            CreateMap<BasketCheckout, BasketCheckoutEvent>().ReverseMap();
        }
    }
}
