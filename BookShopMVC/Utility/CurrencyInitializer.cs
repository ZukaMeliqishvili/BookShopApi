using Newtonsoft.Json;
using System;

namespace BookShopMVC.Utility
{
    public class CurrencyInitializer : ICurrencyInitializer
    {
        private readonly string _baseUrl;
        private readonly IHttpClientFactory _clientFactory;
        public CurrencyInitializer(IConfiguration configuration, IHttpClientFactory clientFactory)
        {
            _baseUrl = configuration["ApiBaseURL:url"];
            _clientFactory = clientFactory;
        }
        public async Task Initialize()
        {
            var url = _baseUrl + "/Currency";
            var client = _clientFactory.CreateClient();
            var response1 = await client.GetAsync(url);
            if(response1.IsSuccessStatusCode)
            {
                var jsonString = await response1.Content.ReadAsStringAsync();
                var currencies = JsonConvert.DeserializeObject<List<Currency>>(jsonString);
                CurrencyRates.Currencies.AddRange(currencies);
            }
        }
    }
}
