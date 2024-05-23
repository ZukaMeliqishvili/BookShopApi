using BookShopApi.Dto._Book;
using BookShopApi.Services.BookService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }
        protected string GetCurrencyCodeFromCookies()
        {
            string currencyCode = Request.Cookies["currencyCode"];
            if (String.IsNullOrEmpty(currencyCode))
            {
                currencyCode = "gel";
            }
            return currencyCode;
        }
        [Authorize(Roles = "Admin,Staff")]
        [HttpPost]
        public async Task<IActionResult> AddBook([FromForm]BookDto dto, IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("No file uploaded");
                }

                var fileExtension = Path.GetExtension(file.FileName);
                if (string.IsNullOrEmpty(fileExtension) || !fileExtension.Equals(".jpg", StringComparison.OrdinalIgnoreCase))
                {
                    return BadRequest("Only JPG files are allowed");
                }

                var filePath = Path.Combine("wwwRoot", "Images", file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                await _bookService.AddBook(dto, filePath);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _bookService.GetBooks());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                string currencyCode = GetCurrencyCodeFromCookies();
                return Ok(await _bookService.GetBookById(id, currencyCode));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "Admin,Staff")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, BookUpdateDto dto)
        {
            try
            {
                await _bookService.UpdateBook(id, dto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("booksByCategory/{categoryId}")]
        public async Task<IActionResult> GetBooksByCategory(int categoryId)
        {
           var books =  await _bookService.GetBooksByCategory(categoryId);
            return Ok(books);
        }
        [HttpGet("booksByAuthor/{author}")]
        public async Task<IActionResult> GetBooksByAuthor(string author)
        {
            return Ok(await _bookService.GetBooksByAuthor(author));
        }

        [Authorize(Roles = "Admin,Staff")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveBook(int id)
        {
            //soft delete
           await _bookService.RemoveBook(id);
            return Ok();
        }
        [Authorize(Roles ="Admin,Staff")]
        [HttpPut("restockBook/{id}")]
        public async Task<IActionResult> RestockBook(int id, int Amount)
        {
            try
            {
                await _bookService.RestockBook(id, Amount);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
