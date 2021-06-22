using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Apollo11.Models.Enums
{
    public enum TimeInForce
    {
        [EnumMember(Value = "GTC")]
        GoodTillCancelled,

        [EnumMember(Value = "IOC")]
        ImmediateOrCancel
    }
}
