using BookShopMVC.Models.Category;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BookShopMVC.Models.Book
{
    public class BookResponseModel
    {
        public int Id { get; set; }

        [Required, NotNull]
        [MinLength(5)]
        [MaxLength(250)]
        public string Title { get; set; }

        [Required, NotNull]
        [MinLength(20)]
        [MaxLength(2000)]
        public string Description { get; set; }

        [Required, NotNull]
        [MinLength(5)]
        [MaxLength(250)]
        public string Author { get; set; }

        [Required]
        [Range(1,1000)]
        public decimal Price { get; set; }
        public List<CategoryResponseModel> Categories { get; set; }

        [Required]
        [Range(1, 1000)]
        public int NumberOfPages { get; set; }
        public int AmountInStock { get; set; }
        public string ImageUrl { get; set; }
    }
}
