

using Basket.Core.Entities;

namespace Basket.Core.Repositories
{
    public interface IBasketRepository
    {
        Task<ShoppingCart> GetBasket(string userName);
        Task<ShoppingCart> UpsertBasket(ShoppingCart basket);
        Task DeleteBasket(string userName);
    }
}
