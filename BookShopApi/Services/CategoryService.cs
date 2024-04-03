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

        public async Task Delete(int id)
        {
            var category = await _categoryRepository.GetById(id);
            if(category == null)
            {
                throw new Exception("No category found by given id");
            }
            await _categoryRepository.Delete(category);
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            return await _categoryRepository.GetAll();
        }

        public async Task<Category> GetById(int id)
        {
            var category = await _categoryRepository.GetById(id);
            if(category == null)
            {
                throw new Exception("No category found by given id");
            }
            return category;
        }

        public async Task insert(CategoryDto categoryDto)
        {
            var category = categoryDto.Adapt<Category>();
            await _categoryRepository.Insert(category);
        }
    }
}
