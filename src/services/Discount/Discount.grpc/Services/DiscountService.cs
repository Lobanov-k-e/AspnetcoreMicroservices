using AutoMapper;
using Discount.grpc.Entities;
using Discount.grpc.Protos;
using Discount.grpc.Repositories;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.grpc.Services
{

    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IRepository _repository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public DiscountService(IRepository repository, ILogger logger, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await _repository.GetCoupon(request.ProductName)
                ?? throw new RpcException(new Status(StatusCode.NotFound, $"нет скидки для продукта {request.ProductName}"));
            _logger.LogInformation($"discount for product {coupon.Id} successfully retrieved");
            return _mapper.Map<CouponModel>(coupon);
        }       

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {

            var coupon = _mapper.Map<Coupon>(request.Coupon);
            var result = await _repository.CreateCoupon(coupon);

            if (!result)
                throw new RpcException(new Status(StatusCode.Internal, "не удалось добавить скидку"));

            _logger.LogInformation($"discount for product {coupon.Id} successfully created");

            return _mapper.Map<CouponModel>(coupon);          
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);
            var result = await _repository.UpdateCoupon(coupon);

            if (!result)
                throw new RpcException(new Status(StatusCode.Internal, "не удалось обновить скидку"));

            _logger.LogInformation($"discount for product {coupon.Id} successfully updated");


            return _mapper.Map<CouponModel>(coupon);
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var result = await _repository.DeleteCoupon(request.ProductName);
            if (!result)
            {
                _logger.LogWarning($"can't delete discount for product {request.ProductName}");
                return new DeleteDiscountResponse() { Success = result };
            }
            _logger.LogInformation($"discount for product {request.ProductName} successfully deleted");
            return new DeleteDiscountResponse() { Success = result };
        }        
    }
}
