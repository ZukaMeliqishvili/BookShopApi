using BookShopApi.Dto;
using BookShopApi.Entities;

namespace BookShopApi.Services
{
    public interface ICategoryService
    {
        public Task insert(CategoryDto categoryDto);
        public Task<Category> GetAll();
        public Task<Category> GetById(int id);
        public Task Delete(int id);
    }
}
