using Apollo11.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace Apollo11.Converters
{
    public class CandlestickConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Candlestick);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            var candle = (existingValue as Candlestick ?? new Candlestick());

            var array = JArray.Load(reader);

            candle.OpenTime = array.ElementAtOrDefault(0)?.ToObject<long>(serializer) ?? 0;
            candle.Open = array.ElementAtOrDefault(1)?.ToObject<double>(serializer) ?? 0;
            candle.High = array.ElementAtOrDefault(2)?.ToObject<double>(serializer) ?? 0;
            candle.Low = array.ElementAtOrDefault(3)?.ToObject<double>(serializer) ?? 0;
            candle.Close = array.ElementAtOrDefault(4)?.ToObject<double>(serializer) ?? 0;
            candle.Volume = array.ElementAtOrDefault(5)?.ToObject<double>(serializer) ?? 0;
            candle.CloseTime = array.ElementAtOrDefault(6)?.ToObject<long>(serializer) ?? 0;
            candle.QuoteAssetVolume = array.ElementAtOrDefault(7)?.ToObject<double>(serializer) ?? 0;
            candle.NumberOfTrades = array.ElementAtOrDefault(8)?.ToObject<long>(serializer) ?? 0;
            candle.TakerBuyBaseAssetVolume = array.ElementAtOrDefault(9)?.ToObject<double>(serializer) ?? 0;
            candle.TakerBuyQuoteAssetVolume = array.ElementAtOrDefault(10)?.ToObject<double>(serializer) ?? 0;

            return candle;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var candle = (Candlestick)value;
            serializer.Serialize(writer,
                new[]
                {
                candle.OpenTime,
                candle.Open,
                candle.High,
                candle.Low,
                candle.Close,
                candle.Volume,
                candle.CloseTime,
                candle.QuoteAssetVolume,
                candle.NumberOfTrades,
                candle.TakerBuyBaseAssetVolume,
                candle.TakerBuyQuoteAssetVolume
                });
        }
    }
}
