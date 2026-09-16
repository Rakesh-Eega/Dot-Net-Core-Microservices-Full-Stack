namespace Basket.Application.Responses
{
    public record class ShoppingCartResponse
    {
        public string UserName { get; init; }
        public List<ShoppingCartItemResponse> Items { get; init; }
        public ShoppingCartResponse() 
        {
            UserName = string.Empty;
            Items = new List<ShoppingCartItemResponse>();
        }
        public ShoppingCartResponse(string userName) : this(userName,new List<ShoppingCartItemResponse>())
        {
            UserName = userName;
        }

        public ShoppingCartResponse(string userName, List<ShoppingCartItemResponse> items)
        {
            UserName = userName;
            Items = items;
        }
        public decimal TotalPrice => Items.Sum(i => i.Price * i.Quantity);
    }
}
