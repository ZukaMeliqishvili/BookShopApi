using BookShopMVC.Models.Category;

namespace BookShopMVC.Models.Book
{
    public class BookResponseModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }
        public List<CategoryResponseModel> Categories { get; set; }
        public int NumberOfPages { get; set; }
        public int AmountInStock { get; set; }
        public string ImageUrl { get; set; }
    }
}
