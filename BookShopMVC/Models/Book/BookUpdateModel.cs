namespace BookShopMVC.Models.Book
{
    public class BookUpdateModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }
        public int NumberOfPages { get; set; }
    }
}
