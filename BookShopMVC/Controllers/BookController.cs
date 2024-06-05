using BookShopMVC.Models.Book;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace BookShopMVC.Controllers
{
    public class BookController : Controller
    {
        private readonly string _baseUrl;
        private readonly IHttpClientFactory _clientFactory;
        public BookController(IConfiguration configuration, IHttpClientFactory clientFactory)
        {
            _baseUrl = configuration["ApiBaseURL:url"];
            _clientFactory = clientFactory;
        }
        public async Task<IActionResult> Update(int id)
        {
            string currencyCode = Request.Cookies["currencyCode"] ?? "gel";
            string url = _baseUrl + $"/Book/{id}";
            string json;
            using (HttpClient client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("Cookie", $"currencyCode={currencyCode}");
                HttpResponseMessage response = await client.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    return BadRequest();
                }
                json = await response.Content.ReadAsStringAsync();
            };

            ViewBag.CurrencyCode = currencyCode;
            var book = JsonConvert.DeserializeObject<BookResponseModel>(json);
            return View(book);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Update(BookResponseModel model)
        {
            int titleErrors = ModelState["Title"].Errors.Count;
            int descriptionErrors = ModelState["Description"].Errors.Count;
            int authorErrors = ModelState["Author"].Errors.Count;
            int priceErrors = ModelState["Price"].Errors.Count;
            int numberOfPagesErrors = ModelState["NumberOfPages"].Errors.Count;

            if (titleErrors > 0 || descriptionErrors > 0 || authorErrors > 0 || priceErrors > 0
                || numberOfPagesErrors > 0)
            {
                TempData["Error"] = "Please enter valid model";
                return RedirectToAction("Index","Home");
            }
            
            BookUpdateModel updateModel = new BookUpdateModel()
            {
                Author = model.Author,
                Title = model.Title,
                Description = model.Description,
                NumberOfPages = model.NumberOfPages,
                Price = model.Price,
            };
            string url = _baseUrl + $"/Book/{model.Id}";
            var jwtToken = Request.Cookies["JwtToken"];
            var content = new StringContent(JsonConvert.SerializeObject(updateModel), Encoding.UTF8, "application/json");
            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization= new AuthenticationHeaderValue("Bearer", jwtToken);
            var response = await client.PutAsync(url, content);
            if(response.IsSuccessStatusCode)
            {
                TempData["success"] = "Book successfully Updated";
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> Restock(int bookId, int quantity)
        {
            if (quantity<1 || quantity > 100000)
            {
                TempData["error"] = "Please enter valid Amount";
            }
            var url = _baseUrl + $"/Book/restockBook/{bookId}?Amount={quantity}";
            var jwtToken = Request.Cookies["JwtToken"];
            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var response = await client.PutAsync(url, null);
            if(response.IsSuccessStatusCode)
            {
                TempData["success"] = "Book was successfully restocked";
                return RedirectToAction("Index", "Home");
            }
            TempData["error"] = response.ToString();
            return RedirectToAction("Index", "Home");
        }
    }
}
