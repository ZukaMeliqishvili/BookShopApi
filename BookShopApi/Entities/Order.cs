namespace BookShopApi.Entities
{
    public class Order
    {
        //public int Id { get; set; }
        //public Book Book { get; set; }
        //public int BookId { get; set; }
        //public int Quantity { get; set; }
        //public int UserId { get; set; }
        //public User User { get; set; }
        //public string Currency { get; set; }
        //public DateTime OrderDateTime { get; set; } = DateTime.Now;
        //public decimal TotalPrice {  get; set; }
        public int Id { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public decimal TotalPrice => OrderItems.Sum(item => item.TotalPrice);
        public DateTime OrderDateTime { get; set; } = DateTime.Now;
        public string Currency { get; set; }
    }
}
