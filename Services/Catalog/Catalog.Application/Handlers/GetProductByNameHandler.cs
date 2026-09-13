

using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers
{
    public class GetProductByNameHandler : IRequestHandler<GetProductByNameQuery, IList<ProductResponse>>
    {
        private readonly IProductRepository _productrepo;

        public GetProductByNameHandler(IProductRepository productrepo)
        {
            _productrepo = productrepo;
        }
        public async Task<IList<ProductResponse>> Handle(GetProductByNameQuery request, CancellationToken cancellationToken)
        {
            var productList = await _productrepo.GetProductByName(request.name);
            return productList.ToResponseList();
        }
    }
}
