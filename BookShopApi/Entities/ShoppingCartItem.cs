namespace BookShopApi.Entities
{
    public class ShoppingCartItem
    {
        public int Id { get; set; }
        public Book Book { get; set; }
        public int BookId { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public int Quantity {  get; set; }
        public decimal TotalPrice => Book.Price * Quantity;
    }
}
