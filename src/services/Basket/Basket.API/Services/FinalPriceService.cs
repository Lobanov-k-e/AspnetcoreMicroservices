using Basket.API.Entities;
using Discount.grpc.Protos;
using System;
using System.Threading.Tasks;

namespace Basket.API.Services
{
    public class FinalPriceService
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient _service;
        public FinalPriceService(DiscountProtoService.DiscountProtoServiceClient serviceClient)
        {
            _service = serviceClient ?? throw new ArgumentNullException(nameof(serviceClient));
        }

        public async Task UpdatePrices(ShoppingCart cart)
        {
            foreach (var item in cart.Items)
            {
                var request = new GetDiscountRequest()
                {
                    ProductName = item.ProductName
                };               

                var discount = await _service.GetDiscountAsync(request);

                item.ApplyDiscount(discount.Amount);                
            }
        }

    }
}
