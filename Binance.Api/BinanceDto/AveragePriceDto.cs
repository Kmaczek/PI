using System;
using System.Collections.Generic;
using System.Text;

namespace Binance.Api.BinanceDto
{
    public class AveragePriceDto
    {
        public string Symbol { get; set; }
        public int Minutes { get; set; }
        public decimal Price { get; set; }
    }
}
