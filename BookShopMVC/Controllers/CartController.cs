using BookShopMVC.Models.Cart;
using BookShopMVC.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace BookShopMVC.Controllers
{
    [Authorize(Roles = "User")]
    public class CartController : Controller
    {
        private readonly string _baseUrl;
        private readonly IHttpClientFactory _clientFactory;
        public CartController(IConfiguration configuration, IHttpClientFactory clientFactory)
        {
            _baseUrl = configuration["ApiBaseURL:url"];
            _clientFactory = clientFactory;
        }
        public async Task<IActionResult> Index()
        {
            var ulr = _baseUrl + "/ShoppingCart";
            var jwtToken = Request.Cookies["JwtToken"];
            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var response = await client.GetAsync(ulr);
            if(!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                TempData["error"] = responseContent;
            }
            var json = await  response.Content.ReadAsStringAsync();
            List<ShoppingCartItemResponseModel> cartItems = new List<ShoppingCartItemResponseModel>();
            cartItems = JsonConvert.DeserializeObject<List<ShoppingCartItemResponseModel>>(json);
            var currencyCode = Request.Cookies["CurrencyCode"];
            ViewBag.CurrencyCode = currencyCode;
            if (currencyCode == "gel" || String.IsNullOrEmpty(currencyCode))
            {
                ViewBag.CurrencyRate = 1;
            }
            else
            {
                var rate = CurrencyRates.Currencies.LastOrDefault(x => x.Code == currencyCode).Rate;
                ViewBag.CurrencyRate = rate;
            }
            return View(cartItems);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int id, int quantity)
        {
            if (quantity < 0)
            {
                TempData["error"] = "Please choose right quantity";
                return RedirectToAction("Idex");
            }
            var url = _baseUrl + "/ShoppingCart";
            var jwtToken = Request.Cookies["JwtToken"];
            var cartItem = new ShoppingCartRequestModel { BookId = id, Quantity = quantity };
            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var content = new StringContent(JsonConvert.SerializeObject(cartItem), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                TempData["success"] = "Item has successfully added to cart";

            }
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                TempData["error"] = $"{responseContent}";
            }

            return RedirectToAction("Details", "Home", new { id = id });
        }
        
        public async Task<IActionResult> Remove(int id)
        {
            var url = _baseUrl + $"/ShoppingCart/{id}";
            var jwtToken = Request.Cookies["JwtToken"];
            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var response = await client.DeleteAsync(url);
            if(response.IsSuccessStatusCode)
            {
                TempData["success"] = "The Item has been removed from cart";
            }
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                TempData["error"] = $"{responseContent}";
            }
            string currencyCode = Request.Cookies["currencyCode"] ?? "gel";
            ViewBag.currencyCode = currencyCode;
            return RedirectToAction("Index");

        }
        public async Task<IActionResult> RemoveAll()
        {
            var url = _baseUrl + "/ShoppingCart/RemoveAll";
            var jwtToken = Request.Cookies["JwtToken"];
            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var response = await client.DeleteAsync(url);
            if(response.IsSuccessStatusCode)
            {
                TempData["success"] = "Items has been removed successfully";
            }
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                TempData["error"] = $"{responseContent}";
            }
            return RedirectToAction("Index");
        }
    }
}
