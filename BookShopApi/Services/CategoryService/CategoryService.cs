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
            categoryDto.Name = categoryDto.Name.ToUpper();
            var category = categoryDto.Adapt<Category>();
            await _categoryRepository.Insert(category);
        }

        public async Task Update(int id, CategoryDto dto)
        {
            var category =await _categoryRepository.GetById(id);
            if (category == null)
            {
                throw new Exception("No categroy found by given id");
            }
            category.Name=dto.Name.ToUpper();
            await _categoryRepository.SaveChangesAsync();
        }
    }
}
