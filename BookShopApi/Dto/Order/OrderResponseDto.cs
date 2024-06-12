using BookShopApi.Dto._Book;
using BookShopApi.Entities;

namespace BookShopApi.Dto.Order
{
    public class OrderResponseDto
    {
        public int Id { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderDateTime { get; set; }
        public string Currency { get; set; }
        public int Status { get; set; }
    }
}
