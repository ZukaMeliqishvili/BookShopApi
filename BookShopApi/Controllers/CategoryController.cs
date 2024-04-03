using BookShopApi.Dto;
using BookShopApi.Repository;
using BookShopApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookShopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [Authorize(Roles ="1")]
        [HttpPost]
        public async Task<IActionResult> AddCategory(CategoryDto dto)
        {
           await _categoryService.insert(dto);
            return Ok();
        }
        [Authorize(Roles ="1")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {

        }
    }
}
