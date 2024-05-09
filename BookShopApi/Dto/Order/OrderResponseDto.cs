using BookShopApi.Dto._Book;
using BookShopApi.Entities;

namespace BookShopApi.Dto.Order
{
    public class OrderResponseDto
    {
        public int Id { get; set; }
        public BookGetDto Book { get; set; }
        public int Quantity { get; set; }
        public int UserId { get; set; }
        public string Currency { get; set; }
        public DateTime OrderDateTime { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
