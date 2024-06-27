using BookShopMVC.Models.Book;
using BookShopMVC.Models.Category;
using BookShopMVC.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
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
                    var responseContent = await response.Content.ReadAsStringAsync();
                    TempData["error"] = responseContent;
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
            var responseContent = await response.Content.ReadAsStringAsync();
            TempData["error"] = $"Failed to create category: {responseContent}";
            return RedirectToAction("Index", "Home");
        }
        [Authorize(Roles ="Admin,Staff")]
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
            var responseContent = await response.Content.ReadAsStringAsync();
            TempData["error"] = $"Failed to create category: {responseContent}";
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin,Staff")]
        [HttpPost]
        public async Task<IActionResult> CreateCategory(string categoryName)
        {
            var url = _baseUrl + "/Category";
            var jwtToken = Request.Cookies["JwtToken"];
            var model = new CategoryRequestModel()
            {
                name = categoryName
            };
            using (var client = _clientFactory.CreateClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

                var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["success"] = "Category successfully created";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    TempData["error"] = $"Failed to create category: {responseContent}";
                    return RedirectToAction("Index", "Home");
                }
            }
        }
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Create()
        {
            var url = _baseUrl + "/Category";
            var client = _clientFactory.CreateClient();
            var responseMessage = await client.GetAsync(url);
            BookCreateVM model = new BookCreateVM();
            if(!responseMessage.IsSuccessStatusCode)
            {
                TempData["error"] = "There is no categories exists. please add categories first";
                return RedirectToAction("Index", "Home");
            }
            var responseJson = await responseMessage.Content.ReadAsStringAsync();
            var categories = JsonConvert.DeserializeObject<List<CategoryResponseModel>>(responseJson);
            model.Categories = categories;
            model.Book= new BookCreateModel();
            model.Book.CategoryIds=new List<int>();
            return View(model);
        }

        [Authorize(Roles = "Admin,Staff")]
        [HttpPost]
        public async Task<IActionResult> Create(BookCreateVM model, IFormFile file)
        {
            var url = _baseUrl + "/Book";
            int titleErrors = ModelState["Book.Title"].Errors.Count;
            int descriptionErrors = ModelState["Book.Description"].Errors.Count;
            int amountErrors = ModelState["Book.AmountInStock"].Errors.Count;
            int authorErrors = ModelState["Book.Author"].Errors.Count;
            int numberOfpagesErrors = ModelState["Book.NumberOfPages"].Errors.Count;
            int priceErrors = ModelState["Book.Price"].Errors.Count;
            int categoryErrors = ModelState["Book.CategoryIds"].Errors.Count;
            if(titleErrors>0||descriptionErrors>0||amountErrors>0||authorErrors>0||
                numberOfpagesErrors > 0 || priceErrors > 0 || categoryErrors > 0)
            {
                TempData["error"] = "Book parameters are not valid";
                return RedirectToAction("Index", "Home");
            }
            if (file == null)
            {
                TempData["error"] = "Please upload an Image";
                return RedirectToAction("Index", "Home");
            }
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                model.Book.ImageBase64 = Convert.ToBase64String(memoryStream.ToArray());
            }
            var client = _clientFactory.CreateClient();
            var jwtToken = Request.Cookies["JwtToken"];
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var content = new StringContent(JsonConvert.SerializeObject(model.Book), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                TempData["success"] = "The Book successfully created";
            }
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                TempData["error"] = $"Failed to create book: {responseContent}";
            }
            return RedirectToAction("Index", "Home");
        }

    }
}
