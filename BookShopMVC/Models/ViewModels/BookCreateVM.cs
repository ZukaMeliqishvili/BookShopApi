using BookShopMVC.Models.Book;
using BookShopMVC.Models.Category;

namespace BookShopMVC.Models.ViewModels
{
    public class BookCreateVM
    {
        public BookCreateModel Book { get; set; }
        public List<CategoryResponseModel> Categories { get; set; }
    }
}
