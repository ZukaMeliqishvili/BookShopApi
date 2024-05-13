
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
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly MyDapper _myDapper;
        public OrderService(IOrderRepository orderRepository, IBookRepository bookRepository, ICurrencyRepository currencyRepository, IShoppingCartRepository shoppingCartRepository, MyDapper myDapper)
        {
            _orderRepository = orderRepository;
            _bookRepository = bookRepository;
            _currencyRepository = currencyRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _myDapper = myDapper;
        }
        public async Task MakeOrder(int userId, string currency)
        {
            decimal currencyRate = 1;
            if (currency != "gel")
            {
                var cur = await _currencyRepository.GetByCode(currency);
                currencyRate = cur.Rate;
            }
            var cartItems = await _shoppingCartRepository.GetAll(userId);
            if (cartItems == null || cartItems.Count==0) 
            {
                throw new Exception("There is no items in the cart to makea an order");
            }
            Order order = new Order()
            {
               UserId=userId,
               OrderDateTime=DateTime.Now,
               Currency=currency,
               OrderItems= new List<OrderItem>()
            };
            foreach(var item in cartItems)
            {
                order.OrderItems.Add(new OrderItem()
                {
                    Book = item.Book,
                    BookId = item.BookId,
                    Quantity = item.Quantity,
                    TotalPrice = item.TotalPrice,
                });
            }
            order.TotalPrice=(order.OrderItems.Sum(x=>x.TotalPrice))/currencyRate;
            await _orderRepository.Add(order);
            await _myDapper.RemoveAllItemsFromCart(userId);
        }
        public async Task<IEnumerable<OrderResponseDto>> GetUserOrders(int userId)
        {
            var orders = await _orderRepository.GetUserOrders(userId);
            return orders.Adapt<List<OrderResponseDto>>();
        }
        public async Task<IEnumerable<OrderResponseDtoForAdmin>> GetOrders()
        {
            return (await _orderRepository.GetAll()).Adapt<List<OrderResponseDtoForAdmin>>();
        }

        public async Task<OrderResponseDto> GetOrder(int id, int userId)
        {
            var order = await _orderRepository.GetById(id,userId);
            if(order == null)
            {
                throw new Exception("Order Was not found");
            }
            return order.Adapt<OrderResponseDto>();
        }

        public async Task<OrderResponseDtoForAdmin> GetOrder(int id)
        {
            var order = await _orderRepository.GetById(id);
            if (order == null)
            {
                throw new Exception("Order Was not found");
            }
            return order.Adapt<OrderResponseDtoForAdmin>();
        }
    }
}
