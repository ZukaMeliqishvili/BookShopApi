using BookShopApi.Entities;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace BookShopApi.Repository
{
    public interface IBookRepository
    {
        public Task<IEnumerable<Book>> GetAll();
        public Task<Book> GetById(int id);
        public Task insert(Book book);
        public Task update(Book book);
        public Task deleteById(int id);
    }
}
