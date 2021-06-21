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
        private string _symbol;
        private string _interval;
        private string _limit;

        private readonly string _endpoint = "/api/v3/klines?symbol={0}&interval={1}&limit={2}";

        public ExchangeService()
        {
            _symbol = ConfigurationManager.AppSettings.Get("Symbol");
            _interval = ConfigurationManager.AppSettings.Get("Interval");
            _limit = ConfigurationManager.AppSettings.Get("Limit");
        }
        public async Task<List<Candle>> GetCandlesAsync()
        {
            try
            {
                var client = GetClient();
                var response = await client.GetAsync(string.Format(_endpoint, _symbol, _interval, _limit));
                var candles = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Candle>>(candles);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{nameof(GetCandlesAsync)} an error occured: {ex.Message}");
                throw ex;
            }
        }

        private HttpClient GetClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings.Get("BaseAddress"));
            var key = ConfigurationManager.AppSettings.Get("ApiKey");
            client.DefaultRequestHeaders.Add("X-MBX-APIKEY", key);
            return client;
        }
    }
}
