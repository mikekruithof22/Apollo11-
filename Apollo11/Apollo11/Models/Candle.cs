using System;
using System.Collections.Generic;
using System.Text;

namespace Apollo11.Models
{
    public class Candle
    {
        public string EventType { get; set; }
        public long EventTime { get; set; }
        public string Symbol { get; set; }
        public Kline Kline { get; set; }
    }
}
