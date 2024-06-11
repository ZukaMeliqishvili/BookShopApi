using BookShopApi.Dto._Category;
using BookShopApi.Repository;
using BookShopApi.Services.CategoryService;
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

        [Authorize(Roles = "Admin,Staff")]
        [HttpPost]
        public async Task<IActionResult> AddCategory(CategoryDto dto)
        {
           await _categoryService.insert(dto);
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _categoryService.GetAll());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                return Ok(await _categoryService.GetById(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles ="Admin,Staff")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CategoryDto dto)
        {
            try
            {
                await _categoryService.Update(id, dto);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            

        }
    }
}
