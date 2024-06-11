using BookShopApi.Dto._Category;
using BookShopApi.Entities;

namespace BookShopApi.Services.CategoryService
{
    public interface ICategoryService
    {
        public Task insert(CategoryDto categoryDto);
        public Task<IEnumerable<CategoryGetDto>> GetAll();
        public Task<CategoryGetDto> GetById(int id);
        public Task Update(int id, CategoryDto dto);
    }
}
