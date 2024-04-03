using BookShopApi.Dto;
using BookShopApi.Entities;
using BookShopApi.Repository;
using Mapster;

namespace BookShopApi.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IcategoryRepository _categoryRepository;
        public CategoryService(IcategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Category> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Category> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task insert(CategoryDto categoryDto)
        {
            var category = categoryDto.Adapt<Category>();
            await _categoryRepository.Insert(category);
        }
    }
}
