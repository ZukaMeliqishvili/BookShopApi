using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BookShopMVC.Models.Book
{
    public class BookCreateModel
    {
        [NotNull]
        [MinLength(5)]
        [MaxLength(250)]
        public string Title { get; set; }

        [NotNull]
        [MinLength(5)]
        [MaxLength(2000)]
        public string Description { get; set; }

        [NotNull]
        [MinLength(5)]
        [MaxLength(250)]
        public string Author { get; set; }

        [NotNull]
        [Range(1,1000)]
        public decimal Price { get; set; }

        [NotNull]
        [Range(1, 10000)]
        public int NumberOfPages { get; set; }

        [NotNull]
        [Range(0, 10000)]
        public int AmountInStock { get; set; }

        [NotNull]
        public List<int> CategoryIds { get; set; }
        public string ImageBase64 { get; set; }
    }
}
