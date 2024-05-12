
using BookShopApi.Dto._Order;
using BookShopApi.Dto.Order;
using BookShopApi.Entities;
using BookShopApi.Repository;
using Mapster;
using System.Xml;

namespace BookShopApi.Services._Order
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IBookRepository _bookRepository;
        private readonly ICurrencyRepository _currencyRepository;
        public OrderService(IOrderRepository orderRepository, IBookRepository bookRepository, ICurrencyRepository currencyRepository)
        {
            _orderRepository = orderRepository;
            _bookRepository = bookRepository;
            _currencyRepository = currencyRepository;
        }
        public async Task MakeOrder(int bookId, int userId, OrderRequestDto dto, string currency)
        {
            //decimal currencyRate = 1;
            //if (currency != "gel")
            //{
            //    var cur = await _currencyRepository.GetByCode(currency);
            //    currencyRate = cur.Rate;
            //}
            //var book = await _bookRepository.GetById(bookId);
            //if (book == null)
            //{
            //    throw new Exception("No book found by given Id");
            //}
            //if (book.AmountInStock - dto.Quantity < 0)
            //{
            //    throw new Exception($"There is no given amount of books: {dto.Quantity} in stock");
            //}
            //Order order = new Order()
            //{
            //    BookId = bookId,
            //    UserId = userId,
            //    Quantity = dto.Quantity,
            //    Currency = currency,
            //    TotalPrice = Math.Round(((decimal)dto.Quantity * (book.Price / currencyRate)), 2),
            //};
            //book.AmountInStock -= dto.Quantity;
            //await _orderRepository.Add(order);
            throw new NotImplementedException();
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
