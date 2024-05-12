using BookShopApi.Entities;
using BookShopApi.Dto.ShoppingCart;
namespace BookShopApi.Services.ShoppingCart
{
    public interface IShoppingCartService
    {
        Task<List<ShoppingCartItemResponseDto>> GetAll(int userId);
        Task AddToCart(ShoppingCartItemRequestDto item, int userId);
        Task RemoveFromCart(int id, int userId);
    }
}
