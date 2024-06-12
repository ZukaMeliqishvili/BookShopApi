using BookShopMVC.Models.Book;

namespace BookShopMVC.Models.Order
{
    public class OrderItemModel
    {
        public int Id { get; set; }
        public BookResponseModel Book { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
