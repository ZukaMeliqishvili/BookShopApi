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
        //[HttpGet]
        //[Route("temp")]
        //public async Task Temp()
        //{
        //    Random r = new Random();
        //    int[] categoryIds = new int[]
        //    {
        //        1,2,3,4,5
        //    };
        //    List<BookDto> books = new List<BookDto>();
        //    string[] authors = new string[] { "H.P Lovecraft", "Vazha Pshavela", "Arthur Conan Doyle", "Nodar Dumbadze", "william shakespeare" };
        //    for (int i = 0; i < 51; i++)
        //    {
        //        BookDto book = new BookDto();
        //        book.Title = $"book{i + 1}";
        //        book.Author = authors[r.Next(0, authors.Length)];
        //        book.NumberOfPages = r.Next(50, 501);
        //        book.Description = book.Title + "Description";
        //        book.CategoryIds = new List<int>() { categoryIds[r.Next(0, categoryIds.Length)],
        //            categoryIds[r.Next(0, categoryIds.Length)], categoryIds[r.Next(0, categoryIds.Length)] };
        //        book.Price = r.Next(10, 100);
        //        book.AmountInStock = r.Next(10, 1000);
        //        books.Add(book);
        //    }
        //    foreach (var book in books)
        //    {
        //        await _bookService.AddBook(book);
        //    }
        //}
    }
}
