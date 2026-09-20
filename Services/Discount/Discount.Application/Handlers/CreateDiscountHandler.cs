using Discount.Application.Commands;
using Discount.Application.DTOs;
using Discount.Application.Extentions;
using Discount.Application.Mappers;
using Discount.Core.Repositories;
using Grpc.Core;
using MediatR;

namespace Discount.Application.Handlers
{
    public class CreateDiscountHandler : IRequestHandler<CreateDiscountCommand, CouponDto>
    {
        private readonly IDiscountRepository _discountRepository;

        public CreateDiscountHandler(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }
        public async Task<CouponDto> Handle(CreateDiscountCommand request, CancellationToken cancellationToken)
        {
            var validationerror = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(request.ProductName))
            {
               validationerror.Add("ProductName", "ProductName cannot be null or empty.");
            }
            if (string.IsNullOrEmpty(request.Description))
            {
                validationerror.Add("Description", "Description cannot be null or empty.");
            }
            if (request.Amount<=0)
            {
                validationerror.Add("Amount", "Amount cannot be less than or equal to zero.");
            }
            if(validationerror.Any())
            {
                throw GrpcErrorHelper.CreateValidationException(validationerror);
            }

            var coupon = request.ToEntity();

            var created = await _discountRepository.createDiscount(coupon);
            if(!created)
            {
                throw new RpcException(new Status(StatusCode.Internal, $"could not create discount for product {request.ProductName}"));

            }
            return coupon.ToDto();
        }
    }
}
