using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Model.BinanceModels
{
    public class BinanceVM
    {
        public List<BinanceSymbolValueVM> SymbolsValues { get; set; } = new List<BinanceSymbolValueVM>();

        public decimal TotalValue => SymbolsValues.Sum(x => x.ConvertedPrice);
        public string Status { get; set; } = BinanceStatus.None;

        public static BinanceVM Empty
        {
            get
            {
                return new BinanceVM();
            }

        }
}
}
