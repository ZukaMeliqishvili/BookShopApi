using BookShopApi.Dto._Book;
using BookShopApi.Services.BookService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

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
            if(String.IsNullOrEmpty(currencyCode))
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
                if (!ModelState.IsValid)
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
    }
}
