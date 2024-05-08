using BookShopApi.Dto._Book;

namespace BookShopApi.Services.BookService
{
    public interface IBookService
    {
        Task AddBook(BookDto bookDto);
        Task<BookGetDto> GetBookById(int id, string currencyCode);
        Task<IEnumerable<BookGetDto>> GetBooks();
        Task UpdateBook(int id, BookUpdateDto dto);
    }
}
