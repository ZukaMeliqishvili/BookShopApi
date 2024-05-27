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
        public async Task<IActionResult> AddBook(BookDto dto)
        {
            try
            {
                var imageBytes = Convert.FromBase64String(dto.ImageBase64);
                if(!(imageBytes.Length > 1 && imageBytes[0] == 0xFF && imageBytes[1] == 0xD8))
                {
                    return BadRequest("File must be in jpg format");
                }
                string imageName = $"{Guid.NewGuid()}.jpg";
                var filePath = Path.Combine("wwwroot", "Images", imageName);
                var imageUrl = Path.Combine("Images", imageName).Replace('\\', '/');
                System.IO.File.WriteAllBytes(filePath, imageBytes);
                await _bookService.AddBook(dto, imageUrl);
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
