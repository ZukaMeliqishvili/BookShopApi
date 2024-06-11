using BookShopApi.Entities;

namespace BookShopApi.Repository
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAll();
        Task Insert(Category category);
        Task Delete(Category category);
        Task<Category> GetById(int id);
        Task<List<Category>> GetCategoriesByBookId(int id);
        Task SaveChangesAsync();
    }
}
