

using Basket.Application.Commands;
using Basket.Application.Responses;
using Basket.Core.Entities;

namespace Basket.Application.Mappers
{
    public static class BasketMapper
    {
        public static ShoppingCartResponse ToResponse(this ShoppingCart shoppingCart)
        {
            return new ShoppingCartResponse
            {
                UserName = shoppingCart.UserName,
                Items = shoppingCart.Items.Select(item => new ShoppingCartItemResponse
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Price = item.Price,
                    ImageFile = item.ImageFile,
                    Quantity = item.Quantity
                }).ToList(),

            };
        }

        //delegate Way
        public static ShoppingCartResponse ToResponseDelegate(this ShoppingCart shoppingCart) =>
            MapCart(shoppingCart);
        public static readonly Func<ShoppingCart, ShoppingCartResponse> MapCart =
      cart => new ShoppingCartResponse
     {
         UserName = cart.UserName,
         Items = cart.Items.Select(item => new ShoppingCartItemResponse
         {
             ProductId = item.ProductId,
             ProductName = item.ProductName,
             Price = item.Price,
             ImageFile = item.ImageFile,
             Quantity = item.Quantity
         }).ToList()
     };

        public static ShoppingCart ToEntity(this CreateShoppingCartCommand command) 
        {
            return new ShoppingCart
            {
                UserName = command.UserName,
                Items = command.Items.Select(item => new ShoppingCartItem
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    ImageFile = item.ImageFile,
                    Price = item.Price,
                    Quantity = item.Quantity
                }).ToList()
            };
        }
    }
}

    

