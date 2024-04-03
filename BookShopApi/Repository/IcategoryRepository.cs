using BookShopApi.Entities;

namespace BookShopApi.Repository
{
    public interface IcategoryRepository
    {
        Task<IEnumerable<Category>> GetAll();
        Task Insert(Category category);
        Task Delete(Category category);
        Task<Category> GetById(int id);
    }
}
