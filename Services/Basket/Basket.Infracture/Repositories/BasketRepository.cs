

using Basket.Core.Entities;
using Basket.Core.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.Infracture.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _rediscache;

        public BasketRepository(IDistributedCache rediscache)
        {
            _rediscache =rediscache;
        }
        public async Task DeleteBasket(string userName)
        {
            await _rediscache.RemoveAsync(userName);
        }

        public async Task<ShoppingCart> GetBasket(string userName)
        {
            var basket = await _rediscache.GetStringAsync(userName);
            if (string.IsNullOrEmpty(basket))
                return null;

            return JsonSerializer.Deserialize<ShoppingCart>(basket);
        }
        

        public async Task<ShoppingCart> UpsertBasket(ShoppingCart basket)
        {
            var json = JsonSerializer.Serialize(basket);
            await _rediscache.SetStringAsync(basket.UserName, json);
            return await GetBasket(basket.UserName);
        }
    }
}
