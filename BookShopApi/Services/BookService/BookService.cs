using BookShopApi.Dto.Book;
using BookShopApi.Entities;
using BookShopApi.Repository;
using Mapster;

namespace BookShopApi.Services.BookService
{
    public class BookService:IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly ICategoryRepository _categoryRepository;

        public BookService(IBookRepository bookRepository, ICategoryRepository categoryRepository)
        {
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
        }
        public async Task AddBook(BookDto bookDto)
        {
            var book = new Book()
            {
                Title = bookDto.Title,
                Author = bookDto.Author,
                Description = bookDto.Description,
                Price = bookDto.Price,
            };
            await _bookRepository.insert(book);
            foreach(int id in bookDto.CategoryIds)
            {
                var category = await _categoryRepository.GetById(id);
                if(category == null) 
                {
                    throw new Exception($"Category by given id:{id} does not exists");
                }
               await _bookRepository.AddBookCategories(new BookCategories() {Book=book, CategoryId=category.Id });
            }
            await _bookRepository.SaveChangesAsync();
        }
        public async Task<IEnumerable<BookGetDto>> GetBooks()
        {
            var books = (await _bookRepository.GetAll()).Adapt<List<BookGetDto>>();
            //for (int i = 0; i < books.Count; ++i)
            //{
            //    var categories = await _categoryRepository.GetCategoriesByBookId(books[i].Id);
            //    books[i].Categories = categories;
            //}
            if(books !=null)
            {
                foreach (var book in books)
                {
                    var categories = await _categoryRepository.GetCategoriesByBookId(book.Id);
                    book.Categories = categories;
                }
                
            }
            return books;

        }
        public async Task<BookGetDto> GetBookById(int id)
        {
            var book = await _bookRepository.GetById(id);
            if(book ==null)
            {
                throw new Exception("No book found by given id");
            }
            var categories = await _categoryRepository.GetCategoriesByBookId(id);
            var bookDto = book.Adapt<BookGetDto>();
            bookDto.Categories = categories;
            return bookDto;
        }
        public async Task UpdateBook(int id, BookUpdateDto dto)
        {
            var book = await _bookRepository.GetById(id);
            if(book ==null )
            {
                throw new Exception("Book does not exists");
            }
            book.Title = dto.Title;
            book.Author= dto.Author;
            book.Description = dto.Description;
            book.Price = dto.Price;
            await _bookRepository.update(book);
        }
    }
}
