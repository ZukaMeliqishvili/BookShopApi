using BookShopMVC.Models.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace BookShopMVC.Controllers
{
    public class OrderController : Controller
    {
        private readonly string _baseUrl;
        private readonly IHttpClientFactory _clientFactory;
        public OrderController(IConfiguration configuration, IHttpClientFactory clientFactory)
        {
            _baseUrl = configuration["ApiBaseURL:url"];
            _clientFactory = clientFactory;
        }
        [Authorize(Roles = "User")]
        public async Task<IActionResult> MakeOrder()
        {
            var url = _baseUrl + "/Order";
            var jwtToken = Request.Cookies["JwtToken"];
            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var response = await client.PostAsync(url, null);
            if (response.IsSuccessStatusCode)
            {
                TempData["success"] = "The order was made successfully";
                return RedirectToAction("Index");
            }
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                TempData["error"] = responseContent;
                return RedirectToAction("Index", "Cart");
            }

        }
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Index()
        {
            var url = _baseUrl + "/Order";
            var jwtToken = Request.Cookies["JwtToken"];
            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                TempData["error"] = responseContent;
                return RedirectToAction("Index", "Home");
            }
            List<OrderResponseModel> orders = new List<OrderResponseModel>();
            var responseJson = await response.Content.ReadAsStringAsync();
            orders = JsonConvert.DeserializeObject<List<OrderResponseModel>>(responseJson);
            return View(orders);
        }
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Details(int id)
        {
            var url = _baseUrl + $"/Order/userOrder/{id}";
            var jwtToken = Request.Cookies["JwtToken"];
            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                TempData["error"] = responseContent;
                return RedirectToAction("Index");
            }
            var responseJson = await response.Content.ReadAsStringAsync();
            var order = JsonConvert.DeserializeObject<OrderResponseModel>(responseJson);
            return View(order);
        }

        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> ManageOrders()
        {
            var url = _baseUrl + "/Order/Admin";
            var jwtToken = Request.Cookies["JwtToken"];
            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                TempData["error"] = responseContent;
                return RedirectToAction("Index", "Home");
            }
            var responseJson = await response.Content.ReadAsStringAsync();
            var orders = JsonConvert.DeserializeObject<List<OrderResponseModelForAdmin>>(responseJson);
            return View(orders);
        }

        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> OrderDetails(int id)
        {
            var url = _baseUrl + $"/Order/{id}";
            var jwtToken = Request.Cookies["JwtToken"];
            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                TempData["error"] = responseContent;
                return RedirectToAction("ManageOrders");
            }
            var responseJson = await response.Content.ReadAsStringAsync();
            var orders = JsonConvert.DeserializeObject<OrderResponseModelForAdmin>(responseJson);
            return View(orders);
        }
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> ChangeStatus(int id)
        {
            var url = _baseUrl + $"/Order/ChangeStatus/{id}";
            var jwtToken = Request.Cookies["JwtToken"];
            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var response = await client.PutAsync(url, null);
            if (!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                TempData["error"] = responseContent;
                
            }
            else
            {
                TempData["success"] = "Order status has been updated Successfully";
            }
            return RedirectToAction("ManageOrders");
        }
    }
}
