using BookShopApi.Entities;
using BookShopApi.Repository;
using BookShopApi.Services._Order;
using BookShopApi.Services.BookService;
using BookShopApi.Services.CategoryService;
using BookShopApi.Services.ShoppingCart;
using BookShopApi.Services.UserServices;


namespace BookShopApi.Infrastructure.Extensions
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
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
            services.AddScoped<IShoppingCartService, ShoppingCartService>();
            services.AddScoped<IDbInitializer, DbInitializer>();
            IConfiguration configuration = new ConfigurationBuilder()
       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
       .Build();

            services.AddSingleton(configuration);
            services.AddTransient<MyDapper>();
        }
    }
}
