using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace BookShopApi.Dto.Book
{
    public class BookDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        [Range(1,1000)]
        public decimal Price { get; set; }
        public List<int> CategoryIds { get; set; }
    }
}
