using BookShopApi.Dto._Order;
using BookShopApi.Dto.Order;
using BookShopApi.Entities;

namespace BookShopApi.Services._Order
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderResponseDtoForAdmin>> GetOrders();
        Task<IEnumerable<OrderResponseDto>> GetUserOrders(int userId);
        Task MakeOrder(int userId, string currency);
        Task<OrderResponseDto> GetOrder(int id, int userId);
        Task<OrderResponseDtoForAdmin> GetOrder(int id);
    }
}
