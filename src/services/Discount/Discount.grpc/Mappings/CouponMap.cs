using AutoMapper;
using Discount.grpc.Entities;
using Discount.grpc.Protos;

namespace Discount.grpc.Mappings
{
    public class CouponMap : Profile
    {
        public CouponMap()
        {
            CreateMap<Coupon, CouponModel>().ReverseMap();           
        }
    }
}
