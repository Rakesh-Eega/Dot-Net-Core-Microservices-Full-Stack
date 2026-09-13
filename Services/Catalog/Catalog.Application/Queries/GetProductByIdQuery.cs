

using Catalog.Application.Responses;
using Catalog.Core.Entities;
using MediatR;

namespace Catalog.Application.Queries
{
    public record GetProductByIdQuery(string Id):IRequest<ProductResponse>
    {
    }
}
