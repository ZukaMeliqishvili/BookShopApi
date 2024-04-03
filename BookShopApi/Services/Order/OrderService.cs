using BookShopApi.Dto._Order;
using BookShopApi.Entities;
using BookShopApi.Repository;

namespace BookShopApi.Services._Order
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IBookRepository _bookRepository;
        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task MakeOrder(int bookId, int userId, OrderRequestDto dto)
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
                Currency = dto.Currency,
            };
            await _orderRepository.Add(order);
        }
        public async Task<IEnumerable<Order>> GetUserOrders(int userId)
        {
            return await _orderRepository.GetUserOrders(userId);
        }
        public async Task<IEnumerable<Order>> GetOrders()
        {
            return await _orderRepository.GetAll();
        }
    }
}
