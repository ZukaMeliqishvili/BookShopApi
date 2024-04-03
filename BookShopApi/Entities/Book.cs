using System.Collections.Generic;
using System.ComponentModel;

namespace BookShopApi.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author {  get; set; }
        public decimal Price { get; set; }
        public ICollection<BookCategories> Categories { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
