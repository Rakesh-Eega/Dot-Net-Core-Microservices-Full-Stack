using Discount.Application.Commands;
using Discount.Application.DTOs;
using Discount.Core.Entities;
using Discount.Grpc.Protos;

namespace Discount.Application.Mappers
{
    public static class CouponMapper
    {
        public static CouponDto ToDto(this Coupon coupon)
        {
            return new CouponDto(coupon.Id, coupon.ProductName, coupon.Description,coupon.Amount);        
        }

        public static Coupon ToEntity(this CreateDiscountCommand command)
        {
            return new Coupon
            {
                ProductName = command.ProductName,
                Description = command.Description,
                Amount = command.Amount
            };
        }
        public static Coupon ToEntity(this UpdateDiscountCommand command)
        {
            return new Coupon
            {
                Id =command.Id,
                ProductName = command.ProductName,
                Description = command.Description,
                Amount = command.Amount
            };
        }

        public static CouponModel ToModel(this CouponDto couponDto)
        {
            return new CouponModel
            {
                Id = couponDto.Id,
                ProductName = couponDto.ProductName,
                Description = couponDto.Description,
                Amount = couponDto.Amount
            };
        }

        public static CreateDiscountCommand ToCreateCommand(this CouponModel couponModel)
        {
            return new CreateDiscountCommand(couponModel.ProductName, couponModel.Description, couponModel.Amount);
            
        }
        public static UpdateDiscountCommand ToUpdateCommand(this CouponModel couponModel)
        {
            return new UpdateDiscountCommand(couponModel.Id, couponModel.ProductName, couponModel.Description, couponModel.Amount);
        }

    }
}
