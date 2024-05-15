using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BookShopApi.Dto._Book
{
    public class BookDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }
        public int NumberOfPages { get; set; }
        public int AmountInStock { get; set; }
        public List<int> CategoryIds { get; set; }
    }
}
