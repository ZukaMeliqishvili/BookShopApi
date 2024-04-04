using BookShopApi.Entities;
using BookShopApi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace BookShopApi.Controllers
{
    [Authorize(Roles ="1")]
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    { 
        private readonly ICurrencyRepository _currencyRepository;

        public CurrencyController(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            string apiUrl = "https://nbg.gov.ge/gw/api/ct/monetarypolicy/currencies/en/json/";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode();
                    string jsonString = await response.Content.ReadAsStringAsync();
                    dynamic data = JArray.Parse(jsonString);

                    var currencies = new List<Currency>();

                    foreach (var currency in data[0].currencies)
                    {
                        currencies.Add(new Currency
                        {
                            Code = currency.code.ToString().ToLower(),
                            Rate = currency.rate
                        });
                    }
                    await _currencyRepository.AddRange(currencies.Where(x => x.Code == "usd" || x.Code == "eur").ToList());
                    return Ok();
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"HTTP error occurred: {e.Message}");
                return NotFound(e.Message);
            }
            catch (Newtonsoft.Json.JsonException e)
            {
                Console.WriteLine($"JSON parsing error occurred: {e.Message}");
                return NotFound(e.Message);
            }
            catch(Exception e)
            {
                return NotFound(e.Message);
            }
        }
    }
}
