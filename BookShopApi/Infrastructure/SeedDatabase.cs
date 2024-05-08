using BookShopApi.Repository;
using BookShopApi.Services.UserServices;

namespace BookShopApi.Infrastructure
{
    public static class SeedDatabase
    {
        public static void Seed(WebApplication app)
        {
            using (IServiceScope scope = app.Services.CreateScope())
                scope.ServiceProvider.GetRequiredService<IDbInitializer>().Initialize();
        }
    }
}
