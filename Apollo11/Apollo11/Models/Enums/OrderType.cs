using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Apollo11.Models.Enums
{
    public enum OrderType
    {
        [EnumMember(Value = "LIMIT")]
        Limit,

        [EnumMember(Value = "MARKET")]
        Market
    }
}
