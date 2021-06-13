using Apollo11.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Apollo11.Services
{
    public class ExchangeService
    {
        private readonly string endpoint = "/fapi/v1/klines";
        public async Task<List<Candle>> GetCandlesAsync()
        {
            var client = GetClient();
            var response = await client.GetAsync(endpoint);
            var candles = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Candle>>(candles);
        }

        private HttpClient GetClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings.Get("BaseAddress"));
            return client;
        }
    }
}
