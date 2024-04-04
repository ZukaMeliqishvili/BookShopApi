using BookShopApi.Dto._Order;
using BookShopApi.Dto.Order;
using BookShopApi.Entities;

namespace BookShopApi.Services._Order
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderResponseDto>> GetOrders();
        Task<IEnumerable<OrderResponseDto>> GetUserOrders(int userId);
        Task MakeOrder(int bookId, int userId, OrderRequestDto dto);
    }
}
