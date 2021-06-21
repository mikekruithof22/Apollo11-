using Apollo11.Services;
using System;

namespace Apollo11
{
    class Program
    {
        static void Main(string[] args)
        {
            var exchangeService = new ExchangeService();
            var candles = exchangeService.GetCandlesAsync().Result;
            // ExchangeService - Get Candles
            // FileService - Save Canldes
            // File Fesvice - Get All Relevant canldes
            // DataProcessor - Calculate RSI
            // DataProcessor - Calculate RSI Delta
            // DataProcessor - CalculatePrice Delta
            // BUY?
            // ExchangeService - Buy Bitcoin
            Console.WriteLine("Hello World!");
        }
    }
}
