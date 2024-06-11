using BookShopMVC.Models.Cart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;

namespace BookShopMVC.Controllers
{
    public class CartController : Controller
    {
        private readonly string _baseUrl;
        private readonly IHttpClientFactory _clientFactory;
        public CartController(IConfiguration configuration, IHttpClientFactory clientFactory)
        {
            _baseUrl = configuration["ApiBaseURL:url"];
            _clientFactory = clientFactory;
        }
        [Authorize(Roles ="User")]
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
            return View(cartItems);
        }
        [Authorize(Roles ="User")]
        [HttpPost]
        public async Task<IActionResult> AddToCart(int id, int quantity)
        {
            if(quantity < 0)
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
                return RedirectToAction("Details", "Home", new { id = id });
            }
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                TempData["error"] = $"responseContent";
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
