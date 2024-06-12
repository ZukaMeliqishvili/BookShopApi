
using BookShopApi.Dto._Order;
using BookShopApi.Dto.Order;
using BookShopApi.Entities;
using BookShopApi.Repository;
using Mapster;
using Microsoft.Extensions.Caching.Distributed;
using System.Xml;

namespace BookShopApi.Services._Order
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IBookRepository _bookRepository;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IDistributedCache _cache;
        private readonly MyDapper _myDapper;
        public OrderService(IOrderRepository orderRepository, IBookRepository bookRepository, ICurrencyRepository currencyRepository, IShoppingCartRepository shoppingCartRepository, MyDapper myDapper, IDistributedCache cache)
        {
            _orderRepository = orderRepository;
            _bookRepository = bookRepository;
            _currencyRepository = currencyRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _myDapper = myDapper;
            _cache = cache;
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
                throw new Exception("There is no items in the cart to make an order");
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
                if(item.Book.AmountInStock < item.Quantity)
                {
                    throw new Exception("there is not enough items in stock");
                }    
                order.OrderItems.Add(new OrderItem()
                {
                    Book = item.Book,
                    BookId = item.BookId,
                    Quantity = item.Quantity,
                    TotalPrice = item.TotalPrice,
                });
                item.Book.AmountInStock -= item.Quantity;
                await UpdateRedisCache(item.BookId);
            }
            order.TotalPrice=(order.OrderItems.Sum(x=>x.TotalPrice))/currencyRate;
            await _orderRepository.Add(order);
            await _myDapper.RemoveAllItemsFromCart(userId);
            await _cache.RemoveAsync("GetBooks");
            
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

        public async Task ProceedOrder(int id)
        {
            var order = await _orderRepository.GetById(id);
            if(order == null)
            {
                throw new NullReferenceException("Order Was not found");
            }
            else if(order.Status==4)
            {
                throw new Exception("OrderIsAlreadyCompleted");
            }
            order.Status += 1;
            await _orderRepository.SaveChangesAsync();
        }
        private async Task UpdateRedisCache(int id)
        {
            string key = $"GetBookById-{id}";
            await _cache.RemoveAsync(key + "-gel");
            await _cache.RemoveAsync(key + "-usd");
            await _cache.RemoveAsync(key + "-eur");
        }
    }
}
