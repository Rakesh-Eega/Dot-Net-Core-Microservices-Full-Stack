namespace Basket.Application.DTOs
{
    public record ShoppingCartDto(
        string UserName,
        IList<ShoppingCartItemDto> Items,
        decimal TotalPrice
    );

    public record ShoppingCartItemDto
    (string ProductId, string ProductName, decimal Price, int Quantity, string ImageFile);
       
    public record CreateShoppingCartItemDto(
        string ProductId,
        string ProductName,
        decimal Price,
        int Quantity,
        string ImageFile
    );
}
