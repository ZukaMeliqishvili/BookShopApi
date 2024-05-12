using BookShopApi.Dto._Book;

namespace BookShopApi.Dto.ShoppingCart
{
    public class ShoppingCartItemResponseDto
    {
        public int Id { get; set; }
        public BookGetDto Book { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice => Book.Price * Quantity;
    }
}
