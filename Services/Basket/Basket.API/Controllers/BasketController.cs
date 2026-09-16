using Basket.Application.Commands;
using Basket.Application.DTOs;
using Basket.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BasketController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{userName}")]
        public async Task<ActionResult<ShoppingCartDto>> GetBasket(string userName)
        {
            var basket = await _mediator.Send(new GetBasketByUserNameQuery(userName));
            return Ok(basket);
        }

        [HttpPost]
        public async Task<ActionResult<ShoppingCartDto>> CreateBasket([FromBody] CreateShoppingCartCommand command)
        {
            var basket = await _mediator.Send(command);
            return Ok(basket);
        }

        [HttpDelete("{userName}")]
        public async Task<ActionResult> DeleteBasket(string userName)
        {
            await _mediator.Send(new DeleteBasketByUserNameCommand(userName));
            return Ok();
        }
    }
}
