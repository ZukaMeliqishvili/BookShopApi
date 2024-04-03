using BookShopApi.Repository;
using BookShopApi.Services.BookService;
using BookShopApi.Services.CategoryService;
using BookShopApi.Services.UserServices;
using Microsoft.AspNetCore.Authorization;

namespace BookShopApi.Extensions
{
    public static class ServiceExtension
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IBookService, BookService>();
        }
    }
}
