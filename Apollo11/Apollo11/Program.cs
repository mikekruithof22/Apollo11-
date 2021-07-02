using Apollo11.Helpers;
using Apollo11.Models;
using Apollo11.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;

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

            Console.WriteLine("Done");
            Console.ReadLine();
        }

        private static void GetDivergences(List<Candlestick> candles, double[] rsis)
        {
            Console.WriteLine($"Number of total candles={candles.Count}");

            // get minimum values for a "good" divergence from the App.config
            var minimumRsiDeltaString = ConfigurationManager.AppSettings.Get("MinimumRsiDelta") ?? throw new SettingsPropertyNotFoundException("No MinimumRsiDelta Provided");
            Int32.TryParse(minimumRsiDeltaString, out int minimumRsiDelta);
            var minimumPriceDeltaString = ConfigurationManager.AppSettings.Get("MinimumPriceDelta") ?? throw new SettingsPropertyNotFoundException("No MinimumPriceDelta Provided");
            Int32.TryParse(minimumPriceDeltaString, out int minimumPriceDelta);

            var backTrackCount = 25; // number of candles back to compare, relative to the "current" candle
            var divergences = new List<Divergence>();

            // to calculate to highest rsi and price deltas of divergences when all candles are processed
            double maxDivergenceRsiDelta = 0; 
            double maxDivergencePriceDelta = 0;

            // to calculate the average rsi and price deltas of divergences when all candles are processed
            double cumulativeDivergenceRsiDelta = 0;
            double cumulativeDivergencePriceDelta = 0;

            // first for loop is to go through each candle and select it as the "current" candle
            for (int j = 0; j < candles.Count; j++)
            {
                var currentCandleNumber = candles.Count - j - 1; // -1 because the array starts at 0
                var currentCandle = candles[currentCandleNumber];
                currentCandle.CandleNumber = currentCandleNumber;
                var currentRsi = rsis[currentCandleNumber];
                var currentPrice = currentCandle.Close;

                // second for loop is to select the "comparison" candle to calculate the deltas relative to the "current" candle
                for (int i = 0; i < backTrackCount; i++)
                {
                    var comparisonCandleNumber = candles.Count - j - 1 - i;

                    if (comparisonCandleNumber < 0) // can't compare to candles that are older than the limit we've set in the config
                    {
                        continue;
                    }

                    var comparisonCandle = candles[comparisonCandleNumber];
                    comparisonCandle.CandleNumber = comparisonCandleNumber;
                    var comparisonRsi = rsis[comparisonCandleNumber];

                    if (comparisonRsi == 0) // the last 14 candles all have an rsi of 0
                    {
                        continue;
                    }

                    var rsiDelta = currentRsi - comparisonRsi;

                    var comparisonPrice = comparisonCandle.Close;
                    var priceDelta = currentPrice - comparisonPrice;

                    if (rsiDelta > 0 && priceDelta < 0) // ANY divergence
                    {
                        var divergence = new Divergence
                        {
                            CurrentCandle = currentCandle,
                            ComparisonCandle = comparisonCandle,
                            RsiDelta = rsiDelta,
                            PriceDelta = priceDelta
                        };

                        divergences.Add(divergence); // add the divergence to the master list

                        if (rsiDelta > maxDivergenceRsiDelta)
                        {
                            maxDivergenceRsiDelta = rsiDelta; // determine the highest rsiDelta
                        }

                        if (priceDelta < maxDivergencePriceDelta)
                        {
                            maxDivergencePriceDelta = priceDelta; // determine the highest priceDelta
                        }

                        cumulativeDivergenceRsiDelta += rsiDelta;
                        cumulativeDivergencePriceDelta += priceDelta;

                        // filtering "good" divergences with minimal levels for the deltas
                        if (rsiDelta > minimumRsiDelta) // arbitrary number, configurable in the App.config
                        {
                            if (priceDelta < minimumPriceDelta) // same
                            {
                                DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                                DateTime currentDate = start.AddMilliseconds(currentCandle.CloseTime).ToLocalTime();
                                DateTime comparisonDate = start.AddMilliseconds(comparisonCandle.CloseTime).ToLocalTime();

                                Console.WriteLine($"Good divergence found! {nameof(rsiDelta)}={rsiDelta} and {nameof(priceDelta)}={priceDelta}");

                                // we need the dateTimes to manually find the divergences in the graph
                                Console.WriteLine($"current candle dateTime={currentDate}");
                                Console.WriteLine($"Comparison candle dateTime={comparisonDate}");

                                // Console.ReadLine();


                                // starting buy order evaluation
                                var futureCandles = candles.Count - j;

                                Console.WriteLine($"current candle ={j}");
                                Console.WriteLine($"number of future candles ={futureCandles}");

                                // get minimum values for a "good" divergence from the App.config
                                //var stopLossPercentageString = ConfigurationManager.AppSettings.Get("StopLossPercentage") ?? throw new SettingsPropertyNotFoundException("No StopLossPercentage Provided");
                                //Double.TryParse(minimumRsiDeltaString, out double StopLossPercentage);
                                //var stopProfitPercentageString = ConfigurationManager.AppSettings.Get("StopProfitPercentage") ?? throw new SettingsPropertyNotFoundException("No StopProfitPercentage Provided");
                                //Double.TryParse(minimumPriceDeltaString, out double StopProfitPercentage);

                                for (int k = 0; k < futureCandles; k++)
                                {
                                    var futureComparisonCandleNumber = j + k;

                                    if (futureComparisonCandleNumber >= candles.Count)
                                    {
                                        continue;
                                    }

                                    var futureComparisonCanlde = candles[futureComparisonCandleNumber];

                                    if (futureComparisonCanlde.High > currentPrice * 1.02)
                                    {

                                        Console.WriteLine($"PROFIT! future high bigger dan profit margin: high={futureComparisonCanlde.High} difference={futureComparisonCanlde.High - currentPrice}");
                                        Console.WriteLine();
                                        Console.WriteLine();
                                        break;
                                    }

                                    if (futureComparisonCanlde.Low < currentPrice * 0.98)
                                    {
                                        Console.WriteLine($"LOSS! future low smaller dan stop loss: low={futureComparisonCanlde.Low} difference={currentPrice - futureComparisonCanlde.Low}");
                                        Console.WriteLine();
                                        Console.WriteLine();
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            var numberOfDivergences = divergences.Count;
            var averageDivergenceRsiDelta = cumulativeDivergenceRsiDelta / numberOfDivergences;
            var averageDivergencePriceDelta = cumulativeDivergencePriceDelta / numberOfDivergences;

            Console.WriteLine($"{nameof(numberOfDivergences)}={numberOfDivergences}");

            Console.WriteLine($"{nameof(maxDivergenceRsiDelta)}={maxDivergenceRsiDelta},");

            Console.WriteLine($"{nameof(maxDivergencePriceDelta)}={maxDivergencePriceDelta},");

            Console.WriteLine($"{nameof(averageDivergenceRsiDelta)}={averageDivergenceRsiDelta},");

            Console.WriteLine($"{nameof(averageDivergencePriceDelta)}={averageDivergencePriceDelta}");
        }
    }

    public class Divergence
    {
        public double RsiDelta { get; set; }
        public double PriceDelta { get; set; }
        public Candlestick CurrentCandle { get; set; }
        public Candlestick ComparisonCandle { get; set; }
    }
}
