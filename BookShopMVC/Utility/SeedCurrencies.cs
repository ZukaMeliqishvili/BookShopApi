namespace BookShopMVC.Utility
{
    public class SeedCurrencies
    {

            public static void Seed(WebApplication app)
            {
                using (IServiceScope scope = app.Services.CreateScope())
                    scope.ServiceProvider.GetRequiredService<ICurrencyInitializer>().Initialize();
            }
    }
}
