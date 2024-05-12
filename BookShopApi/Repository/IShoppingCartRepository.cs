using BookShopApi.Entities;

namespace BookShopApi.Repository
{
    public interface IShoppingCartRepository
    {
        Task Add(ShoppingCartItem item);
        Task Delete(ShoppingCartItem item);
        Task<List<ShoppingCartItem>> GetAll(int userId);
        Task<ShoppingCartItem> Get(int id,int userId);
        Task<ShoppingCartItem> GetByBookId(int id, int userId);
        Task SaveChangesAsync();
    }
}
