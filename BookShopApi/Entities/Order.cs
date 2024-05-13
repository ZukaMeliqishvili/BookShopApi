namespace BookShopApi.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderDateTime { get; set; } = DateTime.Now;
        public string Currency { get; set; }
    }
}
