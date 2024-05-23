using BookShopApi.Dto._Book;
using BookShopApi.Entities;
using BookShopApi.Repository;
using Mapster;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace BookShopApi.Services.BookService
{
    public class BookService:IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly MyDapper _myDapper;
        private readonly IDistributedCache _cache;
        public BookService(IBookRepository bookRepository, ICategoryRepository categoryRepository, ICurrencyRepository currencyRepository, MyDapper myDapper, IDistributedCache cache)
        {
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
            _currencyRepository = currencyRepository;
            _myDapper = myDapper;
            _cache = cache;
        }
        public async Task AddBook(BookDto bookDto, string imageUrl)
        {
            var book = bookDto.Adapt<Book>();
            book.ImageUrl = imageUrl;
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
            await _cache.RemoveAsync("GetBooks");
            await _bookRepository.SaveChangesAsync();
        }
        public async Task<IEnumerable<BookGetDto>> GetBooks()
        {
            var cacheKey = "GetBooks";
            var cacheData = await _cache.GetStringAsync(cacheKey);
            if(!string.IsNullOrEmpty(cacheData))
            {
                List<BookGetDto> dto = JsonConvert.DeserializeObject<List<BookGetDto>>(cacheData);
                return dto;
            }
            var books = (await _bookRepository.GetAll()).Adapt<List<BookGetDto>>();
            var chacheOptions = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                .SetAbsoluteExpiration(TimeSpan.FromHours(0.5));
            await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(books), chacheOptions);
            return books;
        }
        public async Task<BookGetDto> GetBookById(int id, string currencyCode)
        {
            var cacheKey = $"GetBookById-{id}-{currencyCode}";
            var cacheData = await _cache.GetStringAsync(cacheKey);
            BookGetDto bookDto;
            if(!string.IsNullOrEmpty(cacheData))
            {
                bookDto =  JsonConvert.DeserializeObject<BookGetDto>(cacheData);
                return bookDto;
            }
            var book = await _bookRepository.GetById(id);
            if(book ==null)
            {
                throw new Exception("No book found by given id");
            }
            var categories = await _categoryRepository.GetCategoriesByBookId(id);
            bookDto = book.Adapt<BookGetDto>();
            bookDto.Price=book.Price;
            bookDto.Categories = categories;
            await AssignPrice(bookDto, currencyCode);
            var chacheOptions = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                .SetAbsoluteExpiration(TimeSpan.FromHours(0.5));
            await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(bookDto), chacheOptions);
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
            await UpdateRedisCache(id);
        }
        private async Task AssignPrice(BookGetDto book, string currencyCode)
        {

            if (currencyCode != "gel")
            {
                var currency = await _currencyRepository.GetByCode(currencyCode);

                if (currency == null)
                {
                    throw new Exception("Invalid Currency code");
                }
                book.Price = Math.Round(book.Price / currency.Rate, 2);
            }
        }
        public async Task<IEnumerable<BookGetDto>> GetBooksByCategory(int categoryId)
        {
            List<BookGetDto> books;
           var cacheData = await _cache.GetStringAsync("GetBooks");
            if (!string.IsNullOrEmpty(cacheData))
            {
                books = JsonConvert.DeserializeObject<List<BookGetDto>>(cacheData);
                return books.Where(b => b.Categories.Any(x=>x.Id==categoryId));
            }

            books = (await _bookRepository.GetBooksByCategory(categoryId)).Adapt<List<BookGetDto>>();
            return books;
        }

        public async Task RemoveBook(int id)
        {
            await _myDapper.ExecDeleteBookPrecedure(id);
            await UpdateRedisCache(id);
        }

        public async Task<List<BookGetDto>> GetBooksByAuthor(string author)
        {
            var books = await _myDapper.ExecFindBooksByAuthorProcedure(author);
            return books.Adapt<List<BookGetDto>>();
        }

        public async Task RestockBook(int id, int amount)
        {
            if(amount<1 || amount > 100000)
            {
                throw new Exception("Wrong Amount");
            }
            await _myDapper.RestockBook(id, amount);
            await UpdateRedisCache(id);
        }
        private async Task UpdateRedisCache(int id)
        {
            string key = $"GetBookById-{id}";
            await _cache.RemoveAsync(key + "-gel");
            await _cache.RemoveAsync(key + "-usd");
            await _cache.RefreshAsync(key + "-eur");
            await _cache.RemoveAsync("GetBooks");
        }
    }
}
