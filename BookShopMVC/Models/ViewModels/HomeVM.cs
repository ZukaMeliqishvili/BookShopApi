using BookShopMVC.Models.Book;
using BookShopMVC.Models.Category;
using X.PagedList;

namespace BookShopMVC.Models.ViewModels
{
    public class HomeVM
    {
        public IPagedList<BookResponseModel> Books { get; set; }
        public List<CategoryResponseModel> Categories { get; set; }
    }
}
