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
    }
}
