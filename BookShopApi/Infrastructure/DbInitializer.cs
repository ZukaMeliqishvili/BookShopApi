using BookShopApi.Dto.User;
using BookShopApi.Entities;
using BookShopApi.Services.UserServices;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace BookShopApi.Infrastructure
{
    public class DbInitializer : IDbInitializer
    {
        private readonly BookShopContext _context;
        private readonly IUserService _userService;

        public DbInitializer(BookShopContext contex, IUserService userService)
        {
            _context = contex;
            _userService = userService;
        }

        public void Initialize()
        {
            try
            {
                if (_context.Database.GetPendingMigrations().Count() > 0)
                {
                    _context.Database.Migrate();
                }
            }
            catch
            {

            }
            if (!(_context.Roles.Count() > 0))
            {
                _context.Roles.Add(new Role { Name = "Admin" });
                _context.Roles.Add(new Role { Name = "Staff" });
                _context.Roles.Add(new Role { Name = "User" });
                _context.SaveChanges();
                var user = new UserRegisterDto()
                {
                    FirstName = "Jon",
                    LastName = "Lord",
                    UserName = "Admin",
                    Password = "Admin123",
                    Email = "bookShopAdmin@gmail.com",
                    Address = "Khashuri",
                    PhoneNumber = "+995000000000",
                };
                _userService.Register(user).Wait();
               var user1 = _context.Users.FirstOrDefault(x => x.UserName == "Admin");
                user1.RoleId = 1;
                _context.SaveChanges();
            }
        }
    }
}
