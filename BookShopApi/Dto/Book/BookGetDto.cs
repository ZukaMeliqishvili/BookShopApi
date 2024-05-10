using BookShopApi.Entities;

namespace BookShopApi.Dto._Book
{
    public class BookGetDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }
        public List<Category> Categories { get; set; }
        public int NumberOfPages { get; set; }
        public int AmountInStock { get; set; }
    }
}
