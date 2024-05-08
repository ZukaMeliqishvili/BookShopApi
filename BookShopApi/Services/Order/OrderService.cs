using BookShopApi.Dto._Order;
using BookShopApi.Dto.Order;
using BookShopApi.Entities;
using BookShopApi.Repository;
using Mapster;

namespace BookShopApi.Services._Order
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IBookRepository _bookRepository;
        public OrderService(IOrderRepository orderRepository, IBookRepository bookRepository)
        {
            _orderRepository = orderRepository;
            _bookRepository = bookRepository;
        }
        public async Task MakeOrder(int bookId, int userId, OrderRequestDto dto, string currency)
        {
            var book = await _bookRepository.GetById(bookId);
            if(book == null)
            {
                throw new Exception("No book found by given Id");
            }
            Order order = new Order()
            {
                BookId = bookId,
                UserId = userId,
                Quantity = dto.Quantity,
                Currency = currency,
            };
            await _orderRepository.Add(order);
        }
        public async Task<IEnumerable<OrderResponseDto>> GetUserOrders(int userId)
        {
            var orders = await _orderRepository.GetUserOrders(userId);
            return orders.Adapt<List<OrderResponseDto>>();
        }
        public async Task<IEnumerable<OrderResponseDto>> GetOrders()
        {
            return (await _orderRepository.GetAll()).Adapt<List<OrderResponseDto>>();
        }
    }
}
