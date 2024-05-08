using BookShopApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookShopApi.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly BookShopContext _context;

        public BookRepository(BookShopContext context)
        {
            _context = context;
        }

        public async Task AddBookCategories(BookCategories bookCateogories)
        {
            await _context.BookCategories.AddAsync(bookCateogories);
        }

        public async Task deleteById(int id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(x=> x.Id == id);
            if(book == null)
            {
                return;
            }
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Book>> GetAll()
        {
            return await _context.Books.Include(x=>x.Categories).ThenInclude(y=>y.Category).ToListAsync();
        }

        public async Task<Book> GetById(int id)
        {
           return await _context.Books.Include(x => x.Categories).ThenInclude(y => y.Category).FirstOrDefaultAsync(x=>x.Id == id);

        }

        public async Task insert(Book book)
        {
            await _context.Books.AddAsync(book);
        }
        public async Task update(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
