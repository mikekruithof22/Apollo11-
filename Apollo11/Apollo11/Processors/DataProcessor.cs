using Apollo11.Helpers;
using Apollo11.Models;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace Apollo11.Processors
{
    public class DataProcessor
    {
        private int _numberOfDataPoints;
        public DataProcessor()
        {
            var numberOfDataPointsString = ConfigurationManager.AppSettings.Get("NumberOfDataPoints") ?? throw new SettingsPropertyNotFoundException("No numberOfDataPoints Provided");

            if (Int32.TryParse(numberOfDataPointsString, out int numberOfDataPoints))
            {
                _numberOfDataPoints = numberOfDataPoints;
            };
            throw new SettingsPropertyNotFoundException("numberOfDataPoints is not a number");
        }

        public void ProcessCandles(List<Kline> klines)
        {
            var rsi = RsiHelper.CalculateRelativeStrengthIndex(_numberOfDataPoints, klines);
        }
    }
}
