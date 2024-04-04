using BookShopApi.Entities;

namespace BookShopApi.Repository
{
    public interface ICurrencyRepository
    {
        public Task AddRange(List<Currency> currency);
        public Task<Currency> GetByCode(string code);
    }
}
