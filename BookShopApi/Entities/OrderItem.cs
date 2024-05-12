namespace BookShopApi.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }
        public Book Book { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice => Quantity * Book.Price;
        public int OrderId { get; set; }
        public Order Order { get; set; } 
    }
}
