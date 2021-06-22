using Apollo11.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace Apollo11.Helpers
{
    public class PriceEngine
    {
        public static DataTable data;
        public static double[] positiveChanges;
        public static double[] negativeChanges;
        public static double[] averageGain;
        public static double[] averageLoss;
        public static double[] rsi;

        public static double CalculateDifference(double current_price, double previous_price)
        {
            return current_price - previous_price;
        }

        public static double CalculatePositiveChange(double difference)
        {
            return difference > 0 ? difference : 0;
        }

        public static double CalculateNegativeChange(double difference)
        {
            return difference < 0 ? difference * -1 : 0;
        }

        public static void CalculateRSI(int rsi_period, int price_index = 5)
        {
            for (int i = 0; i < PriceEngine.data.Rows.Count; i++)
            {
                double current_difference = 0.0;
                if (i > 0)
                {
                    double previous_close = PriceEngine.data.Rows[i - 1].Field<double>(price_index);
                    double current_close = PriceEngine.data.Rows[i].Field<double>(price_index);
                    current_difference = CalculateDifference(current_close, previous_close);
                }
                PriceEngine.positiveChanges[i] = CalculatePositiveChange(current_difference);
                PriceEngine.negativeChanges[i] = CalculateNegativeChange(current_difference);

                if (i == Math.Max(1, rsi_period))
                {
                    double gain_sum = 0.0;
                    double loss_sum = 0.0;
                    for (int x = Math.Max(1, rsi_period); x > 0; x--)
                    {
                        gain_sum += PriceEngine.positiveChanges[x];
                        loss_sum += PriceEngine.negativeChanges[x];
                    }

                    PriceEngine.averageGain[i] = gain_sum / Math.Max(1, rsi_period);
                    PriceEngine.averageLoss[i] = loss_sum / Math.Max(1, rsi_period);

                }
                else if (i > Math.Max(1, rsi_period))
                {
                    PriceEngine.averageGain[i] = (PriceEngine.averageGain[i - 1] * (rsi_period - 1) + PriceEngine.positiveChanges[i]) / Math.Max(1, rsi_period);
                    PriceEngine.averageLoss[i] = (PriceEngine.averageLoss[i - 1] * (rsi_period - 1) + PriceEngine.negativeChanges[i]) / Math.Max(1, rsi_period);
                    PriceEngine.rsi[i] = PriceEngine.averageLoss[i] == 0 ? 100 : PriceEngine.averageGain[i] == 0 ? 0 : Math.Round(100 - (100 / (1 + PriceEngine.averageGain[i] / PriceEngine.averageLoss[i])), 5);
                }
            }
        }

        public static double[] Launch(List<Candlestick> candles)
        {
            PriceEngine.data = new DataTable();
            //load {date, time, open, high, low, close} values in PriceEngine.data (6th column (index #5) = close price) here

            PriceEngine.data.Columns.Add("CloseTime", typeof(long));
            PriceEngine.data.Columns.Add("OpenTime", typeof(long));
            PriceEngine.data.Columns.Add("Open", typeof(double));
            PriceEngine.data.Columns.Add("High", typeof(double));
            PriceEngine.data.Columns.Add("Low", typeof(double));
            PriceEngine.data.Columns.Add("Close", typeof(double));

            foreach (var candle in candles)
            {
                PriceEngine.data.Rows.Add(new object[] { candle.CloseTime, candle.OpenTime, candle.Open, candle.High, candle.Low, candle.Close });
            }
            PriceEngine.data.AcceptChanges();

            positiveChanges = new double[PriceEngine.data.Rows.Count];
            negativeChanges = new double[PriceEngine.data.Rows.Count];
            averageGain = new double[PriceEngine.data.Rows.Count];
            averageLoss = new double[PriceEngine.data.Rows.Count];
            rsi = new double[PriceEngine.data.Rows.Count];

            CalculateRSI(14);
            var latestRsi = rsi[rsi.Length - 1];
            return rsi;
        }
    }
}
