using BookShopApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookShopApi.Repository
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly BookShopContext _context;

        public ShoppingCartRepository(BookShopContext context)
        {
            _context = context;
        }

        public async Task Add(ShoppingCartItem item)
        {
           await _context.shoppingCartItems.AddAsync(item);
           await _context.SaveChangesAsync();
        }

        public async Task Delete(ShoppingCartItem item)
        {
            _context.shoppingCartItems.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task<ShoppingCartItem> Get(int id,int userId)
        {
            return await _context.shoppingCartItems.Include(x=>x.Book).FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
        }


        public async Task<List<ShoppingCartItem>> GetAll(int userId)
        {
            return await _context.shoppingCartItems.Where(x=>x.UserId==userId).Include(x=>x.Book)
                .ThenInclude(x=>x.Categories).ThenInclude(x=>x.Category).ToListAsync();
        }

        public async Task<ShoppingCartItem> GetByBookId(int id, int userId)
        {
           return await _context.shoppingCartItems.Include(x=>x.Book).FirstOrDefaultAsync(x => x.BookId == id && x.UserId == userId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
