using System;
using Apollo11.Services;

namespace Apollo11
{
    class Program
    {
        static void Main(string[] args)
        {
            var exchangeService = new ExchangeService();
            var candles = exchangeService.GetCandlesAsync().Result;
            // ExchangeService - Get Candles
            // FileService - Save Canldes, Save calculated RSI
            // File Fesvice - Get All Relevant canldes
            // DataProcessor - Calculate RSI
            // DataProcessor - Calculate RSI Delta
            // DataProcessor - CalculatePrice Delta
            // BUY?
            // ExchangeService - Buy Bitcoin
            Console.WriteLine("Hello World!");
            var fileService = new FileService();
            fileService.EnsureTables();
            fileService.WriteCandleToTable();
        }
    }
}
