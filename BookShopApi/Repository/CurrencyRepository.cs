using BookShopApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookShopApi.Repository
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly BookShopContext _context;

        public CurrencyRepository(BookShopContext context)
        {
            _context = context;
        }

        public async Task AddRange(List<Currency> currency)
        {
            await _context.Currency.AddRangeAsync(currency);
            await _context.SaveChangesAsync();
        }

        public async Task<Currency> GetByCode(string code)
        {
            return await _context.Currency.OrderBy(x=>x.Id).LastOrDefaultAsync(x => x.Code == code);
        }
    }
}
