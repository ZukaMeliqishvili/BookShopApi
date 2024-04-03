using BookShopApi.Dto.Book;
using BookShopApi.Entities;

namespace BookShopApi.Services.BookService
{
    public interface IBookService
    {
        Task AddBook(BookDto bookDto);
        Task<IEnumerable<BookGetDto>> GetBooks();
    }
}
