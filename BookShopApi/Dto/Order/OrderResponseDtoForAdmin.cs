using BookShopApi.Dto.Order;
using BookShopApi.Dto.User;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BookShopApi.Dto._Order
{
    public class OrderResponseDtoForAdmin
    {
        public int Id { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderDateTime { get; set; }
        public string Currency { get; set; }
        public UserResponseDto User { get; set; }
        public int Status { get; set; }
    }
}
