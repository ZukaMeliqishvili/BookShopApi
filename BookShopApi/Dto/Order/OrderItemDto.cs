using BookShopApi.Dto._Book;
using BookShopApi.Entities;

namespace BookShopApi.Dto.Order
{
    public class OrderItemDto
    {
        public int Id { get; set; }
        public BookGetDto Book { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
