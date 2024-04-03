using BookShopApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookShopApi.Repository
{
    public class CategoryRepository:IcategoryRepository
    {
        private readonly BookShopContext _context;

        public CategoryRepository(BookShopContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task Insert(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Category category)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task<Category> GetById(int id)
        {
           return await _context.Categories.FirstOrDefaultAsync(x=>x.Id==id);
        }
    }
}
