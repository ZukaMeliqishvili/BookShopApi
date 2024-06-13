using BookShopMVC.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace BookShopMVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CurrencyController : Controller
    {
        private readonly string _baseUrl;
        private readonly IHttpClientFactory _clientFactory;
        public CurrencyController(IConfiguration configuration, IHttpClientFactory clientFactory)
        {
            _baseUrl = configuration["ApiBaseURL:url"];
            _clientFactory = clientFactory;
        }
        public async Task<IActionResult> UpdateCurrencies()
        {
            var url = _baseUrl + "/Currency";
            var jwtToken = Request.Cookies["JwtToken"];
            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var response = await client.GetAsync(url + "/fromNBG");
            if (!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                TempData["error"] = responseContent;
                return RedirectToAction("Index", "Home");
            }
            var response1 = await client.GetAsync(url);
            if (!response1.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                TempData["error"] = responseContent;
                return RedirectToAction("Index", "Home");
            }
            var jsonString = await response1.Content.ReadAsStringAsync();
            var currencies = JsonConvert.DeserializeObject<List<Currency>>(jsonString);
            if (currencies == null || currencies.Count == 0)
            {
                TempData["error"] = "Unable to retrive currency information";
                return RedirectToAction("Index", "Home");
            }
            CurrencyRates.Currencies.AddRange(currencies);
            TempData["success"] = "Currencies has been updated successfully";
            return RedirectToAction("Index", "Home");
        }
    }
}
