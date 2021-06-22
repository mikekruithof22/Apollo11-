using Apollo11.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Apollo11.Services
{
    public class ExchangeService
    {
        private string _symbol;
        private string _interval;
        private string _limit;

        public ExchangeService()
        {
            // getCandles
            _symbol = ConfigurationManager.AppSettings.Get("Symbol");
            _interval = ConfigurationManager.AppSettings.Get("Interval");
            _limit = ConfigurationManager.AppSettings.Get("Limit");
        }
        public async Task<List<Candlestick>> GetCandlesAsync()
        {
            try
            {
                var client = GetClient();
                var endpoint = "/api/v3/klines?symbol={0}&interval={1}&limit={2}";
                var response = await client.GetAsync(string.Format(endpoint, _symbol, _interval, _limit));
                var candlesString = await response.Content.ReadAsStringAsync();
                var candles = JsonConvert.DeserializeObject<List<Candlestick>>(candlesString);
                return candles;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{nameof(GetCandlesAsync)} an error occured: {ex.Message}");
                throw ex;
            }
        }

        public async Task<NewOrderResponse> NewOrderAsync()
        {
            try
            {
                var client = GetClient();                
                var endpoint = "/api/v3/order";
                // TODO: fill order
                var order = new Order();
                var response = await client.PostAsJsonAsync(string.Format(endpoint, _symbol, _interval, _limit), order);
                var json = await response.Content.ReadAsStringAsync();
                var newOrderResponse = JsonConvert.DeserializeObject<NewOrderResponse>(json);
                return newOrderResponse;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{nameof(NewOrderAsync)} an error occured: {ex.Message}");
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
