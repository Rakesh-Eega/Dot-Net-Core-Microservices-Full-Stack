using Discount.Application.Commands;
using Discount.Application.Extentions;
using Discount.Core.Repositories;
using Grpc.Core;
using MediatR;
using GrpcStatus = Grpc.Core.Status;

namespace Discount.Application.Handlers
{
    public class DeleteDiscountHandler : IRequestHandler<DeleteDiscountCommand, bool>
    {
        private readonly IDiscountRepository _discountRepository;

        public DeleteDiscountHandler(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }
        public async Task<bool> Handle(DeleteDiscountCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.ProductName))
            {
                var validationerror = new Dictionary<string, string>();
                validationerror.Add("ProductName", "ProductName cannot be null or empty.");
                throw GrpcErrorHelper.CreateValidationException(validationerror);
            }
            bool deleted = await _discountRepository.DeleteDiscount(request.ProductName);
            if (!deleted)
            {
                throw new RpcException(new GrpcStatus(StatusCode.Internal, $"could not delete discount for product {request.ProductName}"));
            }
            return deleted;
        }
    }
}
