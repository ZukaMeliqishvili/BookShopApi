namespace BookShopApi.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public Book Book { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Currency { get; set; }
        public DateTime OrderDateTime { get; set; } = DateTime.Now;
    }
}
