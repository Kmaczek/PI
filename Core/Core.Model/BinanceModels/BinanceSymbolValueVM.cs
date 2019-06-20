using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model.BinanceModels
{
    public class BinanceSymbolValueVM
    {
        public string Symbol { get; set; }

        [FormatAttribute(FormatType.Numeric2)]
        public decimal Amount { get; set; }

        [FormatAttribute(FormatType.Numeric4)]
        public decimal AvgUsdPrice { get; set; }

        [FormatAttribute(FormatType.Numeric2)]
        public decimal ConvertedPrice => Amount * AvgUsdPrice;
    }
}
