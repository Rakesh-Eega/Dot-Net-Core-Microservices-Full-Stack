using Discount.Grpc.Protos;
using Google.Rpc;
using MediatR;
using Grpc.Core;
using Discount.Application.Queries;
using Discount.Application.Mappers;
using Discount.Application.Commands;

namespace Discount.API.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IMediator _mediator;

        public DiscountService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var query = new GetDiscountQuery(request.ProductName);
            var coupon = await _mediator.Send(query);

            return coupon.ToModel();
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var command = request.Coupon.ToCreateCommand();
            var coupon = await _mediator.Send(command);
            return coupon.ToModel();
        } 

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var command = request.Coupon.ToUpdateCommand();
            var coupon = await _mediator.Send(command);
            return coupon.ToModel();
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var command = new DeleteDiscountCommand(request.ProductName);
            var result = await _mediator.Send(command);
            return new DeleteDiscountResponse { Success = result };
        }
    }
}
