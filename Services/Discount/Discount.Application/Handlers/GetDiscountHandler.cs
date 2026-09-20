using Discount.Application.DTOs;
using Discount.Application.Extentions;
using Discount.Application.Mappers;
using Discount.Application.Queries;
using Discount.Core.Repositories;
using Grpc.Core;
using MediatR;

namespace Discount.Application.Handlers
{
    public class GetDiscountHandler : IRequestHandler<GetDiscountQuery, CouponDto>
    {
        private readonly IDiscountRepository _discountRepository;

        public GetDiscountHandler(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }
        public async Task<CouponDto> Handle(GetDiscountQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.ProductName))
            {
                var validationerror =new Dictionary<string, string>
                {
                    { "ProductName", "ProductName cannot be null or empty." }
                };
                throw GrpcErrorHelper.CreateValidationException(validationerror);
            }
            var coupon = await _discountRepository.GetDiscount(request.ProductName);
            if (coupon == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount for product {request.ProductName} not found."));
            }
            return coupon.ToDto();
        }
    }
}
