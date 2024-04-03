using BookShopApi.Entities;

namespace BookShopApi.Repository
{
    public interface IOrderRepository
    {
        Task Add(Order order);
        Task<IEnumerable<Order>> GetAll();
        Task<Order> GetById(int id);
        Task<IEnumerable<Order>> GetUserOrders(int userId);
    }
}
