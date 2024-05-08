using System.ComponentModel.DataAnnotations;

namespace BookShopApi.Dto._Book
{
    public class BookDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        [Range(1,1000)]
        public decimal Price { get; set; }
        [Range(5,10000)]
        public int NumberOfPages { get; set; }
        [Range(0,100000)]
        public int AmountInStock { get; set; }
        public List<int> CategoryIds { get; set; }
    }
}
