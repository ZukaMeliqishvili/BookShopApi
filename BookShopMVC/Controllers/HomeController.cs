using BookShopMVC.Models;
using BookShopMVC.Models.Book;
using BookShopMVC.Models.Category;
using BookShopMVC.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using X.PagedList;

namespace BookShopMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly string baseUrl;
        public HomeController(ILogger<HomeController> logger,IConfiguration configuration)
        {
            _logger = logger;
            baseUrl = configuration["ApiBaseURL:url"];
        }

        public async Task<IActionResult> Index(int? categoryId, int? page = 1)
        {
            string url = baseUrl + "/Book";
            string url1 = baseUrl + "/Category";
            string booksJsonString;
            string categoriesJsonString;
            using (HttpClient client = new HttpClient())
            {
                if(categoryId.HasValue)
                {
                    url += $"/booksByCategory/{categoryId}";
                }
                HttpResponseMessage response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    return BadRequest(response.Content);
                }
                booksJsonString = await response.Content.ReadAsStringAsync();

                HttpResponseMessage response1 = await client.GetAsync(url1);
                if(!response1.IsSuccessStatusCode)
                {
                    return BadRequest(response1.Content);
                }
                categoriesJsonString = await response1.Content.ReadAsStringAsync();
            }
            var books = JsonConvert.DeserializeObject<List<BookResponseModel>>(booksJsonString);
            var categories = JsonConvert.DeserializeObject<List<CategoryResponseModel>>(categoriesJsonString);
            int pageNumber = page ?? 1;
            HomeVM homeVM = new HomeVM()
            {
                Books = books.ToPagedList(pageNumber, 16),
                Categories = categories
            };
            return View(homeVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
