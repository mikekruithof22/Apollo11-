using Apollo11.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Apollo11.Models
{
    [JsonConverter(typeof(CandlestickConverter))]
    public class Candlestick
    {
        public long OpenTime { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public double Volume { get; set; }
        public long CloseTime { get; set; }
        public double QuoteAssetVolume { get; set; }
        public long NumberOfTrades { get; set; }
        public double TakerBuyBaseAssetVolume { get; set; }
        public double TakerBuyQuoteAssetVolume { get; set; }
    }
}
