using BookShopApi.Dto._Book;
using BookShopApi.Entities;
using BookShopApi.Repository;
using Mapster;

namespace BookShopApi.Services.BookService
{
    public class BookService:IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly MyDapper _myDapper;
        public BookService(IBookRepository bookRepository, ICategoryRepository categoryRepository, ICurrencyRepository currencyRepository, MyDapper myDapper)
        {
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
            _currencyRepository = currencyRepository;
            _myDapper = myDapper;
        }
        public async Task AddBook(BookDto bookDto)
        {
            var book = bookDto.Adapt<Book>();
            await _bookRepository.insert(book);
            for(int i=0;i<bookDto.CategoryIds.Count;i++)
            {
                int id = bookDto.CategoryIds[i];
                var category = await _categoryRepository.GetById(id);
                if (category == null)
                {
                    throw new Exception($"Category by given id:{id} does not exists");
                }
                if(bookDto.CategoryIds.GetRange(0,i).Contains(id))
                {
                    continue;
                }
                await _bookRepository.AddBookCategories(new BookCategories() { Book = book, CategoryId = category.Id, Category = category });
            }
            //foreach(int id in bookDto.CategoryIds)
            //{
            //    var category = await _categoryRepository.GetById(id);
            //    if(category == null) 
            //    {
            //        throw new Exception($"Category by given id:{id} does not exists");
            //    }
            //   await _bookRepository.AddBookCategories(new BookCategories() {Book=book, CategoryId=category.Id,Category=category});
            //}
            await _bookRepository.SaveChangesAsync();
        }
        public async Task<IEnumerable<BookGetDto>> GetBooks()
        {
            var books = await _bookRepository.GetAll();
            return books.Adapt<List<BookGetDto>>();
        }
        public async Task<BookGetDto> GetBookById(int id, string currencyCode)
        {
            var book = await _bookRepository.GetById(id);
            if(book ==null)
            {
                throw new Exception("No book found by given id");
            }
            var categories = await _categoryRepository.GetCategoriesByBookId(id);
            var bookDto = book.Adapt<BookGetDto>();
            bookDto.Price =book.Price;
            bookDto.Categories = categories;

            if (currencyCode!="gel")
            {
                var currency = await _currencyRepository.GetByCode(currencyCode);

                if (currency == null)
                {
                    throw new Exception("Invalid Currency code");
                }
                bookDto.Price = Math.Round(bookDto.Price/currency.Rate, 2);
            }
           
            
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
            book.NumberOfPages=dto.NumberOfPages;
            book.UpdatedAt = DateTime.Now;
            await _bookRepository.update(book);
        }

        public async Task<IEnumerable<BookGetDto>> GetBooksByCategory(int categoryId)
        {
            var books = await _bookRepository.GetBooksByCategory(categoryId);
            return books.Adapt<List<BookGetDto>>();
        }

        public async Task RemoveBook(int id)
        {
            await _myDapper.ExecDeleteBookPrecedure(id);
        }

        public async Task<List<BookGetDto>> GetBooksByAuthor(string author)
        {
            var books = await _myDapper.ExecFindBooksByAuthorProcedure(author);
            return books.Adapt<List<BookGetDto>>();
        }
    }
}
