using BookShopMVC.Models.User;

namespace BookShopMVC.Models.Order
{
    public class OrderResponseModelForAdmin
    {
        public int Id { get; set; }
        public List<OrderItemModel> OrderItems { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderDateTime { get; set; }
        public string Currency { get; set; }
        public UserResponseModel User { get; set; }
    }
}
