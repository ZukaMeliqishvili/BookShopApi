using BookShopApi.Dto._Category;
using BookShopApi.Entities;
using BookShopApi.Repository;
using Mapster;

namespace BookShopApi.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task Delete(int id)
        {
            var category = await _categoryRepository.GetById(id);
            if (category == null)
            {
                throw new Exception("No category found by given id");
            }
            await _categoryRepository.Delete(category);
        }

        public async Task<IEnumerable<CategoryGetDto>> GetAll()
        {
            var categories = await _categoryRepository.GetAll();
            return categories.Adapt<IEnumerable<CategoryGetDto>>();
        }

        public async Task<CategoryGetDto> GetById(int id)
        {
            var category = await _categoryRepository.GetById(id);
            if (category == null)
            {
                throw new Exception("No category found by given id");
            }
            return category.Adapt<CategoryGetDto>();
        }

        public async Task insert(CategoryDto categoryDto)
        {
            var category = categoryDto.Adapt<Category>();
            await _categoryRepository.Insert(category);
        }
    }
}
