using BookShopMVC.Models;
using BookShopMVC.Models.Book;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json.Serialization;
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

        public async Task<IActionResult> Index(int? page = 1)
        {
            string url = baseUrl + "/Book";
            string jsonString;
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    return BadRequest(response.Content);
                }
                jsonString = await response.Content.ReadAsStringAsync();
            }
            var books = JsonConvert.DeserializeObject<List<BookRequestModel>>(jsonString);
            int pageNumber = page ?? 1;
            return View(books.ToPagedList(pageNumber, 8));
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
