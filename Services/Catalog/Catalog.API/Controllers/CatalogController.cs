using Catalog.Application.Commands;
using Catalog.Application.DTOs;
using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Core.Specifications;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CatalogController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAllProducts")]
        public async Task<ActionResult<IList<ProductDto>>> GetAllproducts([FromQuery] CatalogSpecParams specParams)
        {
            var query = new GetAllProductsQuery(specParams);
            var res = await _mediator.Send(query);
            return Ok(res);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(string id)
        {
            var query = new GetProductByIdQuery(id);
            var res = await _mediator.Send(query);
            return Ok(res);

        }

        [HttpGet("productName/{ProductName}")]
        public async Task<ActionResult<IList<ProductDto>>> GetProductsByName(string ProductName)
        {
            var query = new GetProductByNameQuery(ProductName);
            var result = await _mediator.Send(query);
            if(result == null || !result.Any())
            {
                return NotFound();
            }
            var dtolist =result.Select(p=>p.ToDto()).ToList();
            return Ok(dtolist);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] CreateProductDto dto) 
        {
            var command = dto.ToCommand();
           var res = await _mediator.Send(command);
            return Ok(res);

        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult> DeleteProduct(string Id) 
        {
            var command = new DeleteProductByIdCommand(Id);
            var res = await _mediator.Send(command);
            if (!res)
            {
                return NotFound();
            }
            return NoContent();

        }

        [HttpPut("{Id}")]
        public async Task<ActionResult> UpdateProduct(string Id,UpdateProductDto DTO)
        {
            var command =  DTO.ToCommand(Id);
            var res = await _mediator.Send(command);
            if (!res)
            {
                return NotFound();

            }
            return NoContent();
        }

        [HttpGet("GetAllBrands")]
        public async Task<ActionResult<IList<BrandDto>>> GetAllbrands()
        {
            var query = new GetAllBrandsQuery();
            var res = await _mediator.Send(query);
            return Ok(res);
        }

        [HttpGet("GetAllTypes")]
        public async Task<ActionResult<IList<TypeDto>>> GetAllTypes()
        {
            var query = new GetAllTypesQuery();
            var res = await  _mediator.Send(query);
            return Ok(res);
        }

        [HttpGet("/brand/{brand}",Name ="GetProductByBrandName")]
        public async Task<ActionResult<IList<ProductDto>>> GetProductByBrand(string brand)
        {
            var query = new GetProductByBrandQuery(brand);
            var res = await _mediator.Send(query);
            return Ok(res);
        }
    }
}
