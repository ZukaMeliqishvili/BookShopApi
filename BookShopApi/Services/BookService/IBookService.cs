using BookShopApi.Dto._Book;
using BookShopApi.Entities;

namespace BookShopApi.Services.BookService
{
    public interface IBookService
    {
        Task AddBook(BookDto bookDto);
        Task<BookGetDto> GetBookById(int id, string currencyCode);
        Task<IEnumerable<BookGetDto>> GetBooks();
        Task<IEnumerable<BookGetDto>> GetBooksByCategory(int categoryId);
        Task UpdateBook(int id, BookUpdateDto dto);
        Task RemoveBook(int id);
        Task<List<BookGetDto>> GetBooksByAuthor(string author);
    }
}
