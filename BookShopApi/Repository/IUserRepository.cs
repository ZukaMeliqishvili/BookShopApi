using BookShopApi.Entities;

namespace BookShopApi.Repository
{
    public interface IUserRepository
    {
        Task<User> GetUser(string userName);
        Task Insert(User category);
        Task<bool> Exists(string Username);
        Task<List<User>> GetAll();
        Task<User> GetById(int id);
        Task SaveChanges();
    }
}
