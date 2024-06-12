﻿using BookShopMVC.Models.Order;
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
        [Authorize(Roles ="User")]
        public async Task<IActionResult> MakeOrder()
        {
            var url = _baseUrl + "/Order";
            var jwtToken = Request.Cookies["JwtToken"];
            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var response = await client.PostAsync(url,null);
            if (response.IsSuccessStatusCode)
            {
                TempData["success"] = "The order was made successfully";
            }
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                TempData["error"] = responseContent;
            }
            return RedirectToAction("Index");
        }
        [Authorize(Roles ="User")]
        public async Task<IActionResult> Index()
        {
            var url = _baseUrl + "/Order";
            var jwtToken = Request.Cookies["JwtToken"];
            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var response = await client.GetAsync(url);
            if(!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                TempData["error"] = responseContent;
                return RedirectToAction("Index","Home");
            }
            List<OrderResponseModel> orders = new List<OrderResponseModel>();
            var responseJson = await response.Content.ReadAsStringAsync();
            orders = JsonConvert.DeserializeObject<List<OrderResponseModel>>(responseJson);
            return View(orders);
        }
        [Authorize(Roles ="User")]
        public async Task<IActionResult> Details(int id)
        {
            var url = _baseUrl + $"/Order/userOrder/{id}";
            var jwtToken = Request.Cookies["JwtToken"];
            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var response = await client.GetAsync(url);
            if(!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                TempData["error"] = responseContent;
            }
            var responseJson = await response.Content.ReadAsStringAsync();
            var order = JsonConvert.DeserializeObject<OrderResponseModel>(responseJson);
            return View(order);
        }
    }
}
