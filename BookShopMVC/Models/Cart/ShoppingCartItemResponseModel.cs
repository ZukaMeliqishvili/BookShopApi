using BookShopMVC.Models.Book;

namespace BookShopMVC.Models.Cart
{
    public class ShoppingCartItemResponseModel
    {
        public int Id { get; set; }
        public BookResponseModel Book { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice => Book.Price * Quantity;
    }
}
