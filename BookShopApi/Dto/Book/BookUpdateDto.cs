using System.ComponentModel.DataAnnotations;

namespace BookShopApi.Dto.Book
{
    public class BookUpdateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        [Range(1,1000)]
        public decimal Price { get; set; }
    }
}
