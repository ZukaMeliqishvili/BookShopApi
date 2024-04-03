using BookShopApi.Dto.Book;
using BookShopApi.Services.BookService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        [Authorize(Roles ="1")]
        [HttpPost]
        public async Task<IActionResult> AddBook(BookDto dto)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest();
                }
                await _bookService.AddBook(dto);
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
    }
}
