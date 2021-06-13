using System;
using System.Collections.Generic;
using System.Text;

namespace Apollo11.Models
{
    public class Kline
    {
        public long StartTime { get; set; }
        public long EndTime { get; set; }
        public string Symbol { get; set; }
        public string Interval { get; set; }
        public int FirstTradeId { get; set; }
        public int LastTradeId { get; set; }
        public string Open { get; set; }
        public string Close { get; set; }
        public string High { get; set; }
        public string Low { get; set; }
        public string Volume { get; set; }
        public int Trades { get; set; }
        public bool Final { get; set; }
        public string QuoteVolume { get; set; }
        public string VolumeActive { get; set; }
        public string QuoteVolumeActive { get; set; }
        public string Ignored { get; set; }
    }
}
