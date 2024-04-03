using BookShopApi.Dto.Book;
using BookShopApi.Entities;

namespace BookShopApi.Services.BookService
{
    public interface IBookService
    {
        Task AddBook(BookDto bookDto);
        Task<BookGetDto> GetBookById(int id);
        Task<IEnumerable<BookGetDto>> GetBooks();
        Task UpdateBook(int id, BookUpdateDto dto);
    }
}
