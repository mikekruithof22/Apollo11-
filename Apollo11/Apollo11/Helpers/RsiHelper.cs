using Apollo11.Models;
using System;
using System.Collections.Generic;

namespace Apollo11.Helpers
{
    public static class RsiHelper
    {
        public static decimal CalculateRelativeStrengthIndex(int numberOfDataPoints, List<Candlestick> hisotircalClines)
        {
            //RSI Forumula from            
            if (hisotircalClines.Count >= numberOfDataPoints)
            {
                decimal changeValue = 0;
                decimal totalGain = 0;
                decimal totalLoss = 0;
                decimal relativeStrength = 0;
                decimal relativeStrengthIndex = 0;
                int daysUp = 0;
                int daysDown = 0;
                for (int i = 0; i < numberOfDataPoints; i++)
                {
                    if (i == 0)
                        changeValue = 0;
                    else
                        changeValue = hisotircalClines[i - 1].CloseTime - hisotircalClines[i].CloseTime;

                    if (changeValue == 0)
                        continue; //skip
                    else if (changeValue > 0)
                    {
                        totalGain = totalGain + hisotircalClines[i - 1].CloseTime;
                        daysUp = daysUp + 1;
                    }
                    else
                    {
                        totalLoss = totalLoss + Math.Abs(hisotircalClines[i - 1].CloseTime);
                        daysDown = daysDown + 1;
                    }
                }

                if (daysDown != 0) //To avoid divide by zero error
                    relativeStrength = (totalGain / daysUp) / (totalLoss / daysDown);
                else
                    relativeStrength = (totalGain / daysUp);
                relativeStrengthIndex = 100 - (100 / (1 + relativeStrength));
                return relativeStrengthIndex;
            }
            else
                return 0;
        }
    }
}
