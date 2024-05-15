using BookShopApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookShopApi.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly BookShopContext _context;

        public UserRepository(BookShopContext context)
        {
            _context = context;
        }

        public async Task<bool> Exists(string Username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == Username);
            if (user == null)
            {
                return false;
            }
            return true;
        }

        public async Task<List<User>> GetAll()
        {
            return await _context.Users.Where(x=>x.RoleId==3).ToListAsync();
        }

        public async Task<User> GetById(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User> GetUser(string userName)
        {
            var user = await _context.Users.Include(x=>x.Role).FirstOrDefaultAsync(x=>x.UserName==userName);
            return user;
        }

        public async Task Insert(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
