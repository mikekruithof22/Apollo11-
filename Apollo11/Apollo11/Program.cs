using Apollo11.Helpers;
using Apollo11.Models;
using Apollo11.Services;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Apollo11
{
    class Program
    {
        static void Main(string[] args)
        {
            var exchangeService = new ExchangeService();
            var candles = exchangeService.GetCandlesAsync().Result;
            var rsis = PriceEngine.Launch(candles);
            // ExchangeService - Get Candles
            // FileService - Save Canldes, Save calculated RSI
            // File Fesvice - Get All Relevant canldes
            // DataProcessor - Calculate RSI
            // DataProcessor - Calculate RSI Delta
            // DataProcessor - CalculatePrice Delta
            // BUY?
            // ExchangeService - Buy Bitcoin
            Console.WriteLine("Hello World!");
            // var fileService = new FileService();
            // fileService.EnsureTables();
            // fileService.WriteCandleToTable();
            // fileService.ReadCellFromTable("");

            GetDivergences(candles, rsis);
        }

        private static void GetDivergences(List<Candlestick> candles, double[] rsis)
        {
//            Console.WriteLine($"lengths: candles={candles.Count} rsis={rsis.Length}");

            var backTrackCount = 25;

            for (int i = 0; i < backTrackCount; i++)
            {
                var matchup = new Matchup();

                var currentRsi = rsis[rsis.Length - 1];
                var comparisonRsi = rsis[rsis.Length - 1];
                var rsiDelta = currentRsi - comparisonRsi;

                var currentPrice = candles[candles.Count - 1].Close;
                var comparisonPrice = candles[candles.Count - 1].Close;
                var PriceDelta = currentPrice - comparisonPrice;

                if (rsiDelta > 0 && PriceDelta < 0)
                {
                    Console.WriteLine($"DIVERGENCE: rsiDelta={rsiDelta} PriceDelta={PriceDelta}");

                }
            }
        }
    }

    public class Matchup
    {
        public double rsiDelta { get; set; }
        public double priceDelta { get; set; }
    }
}
