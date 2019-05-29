using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Model.BinanceModels
{
    public class BinanceVM
    {
        List<BinanceSymbolValueVM> symbolsValues = new List<BinanceSymbolValueVM>();
        public List<BinanceSymbolValueVM> SymbolsValues
        {
            get { return symbolsValues; }
            set
            {
                symbolsValues.Clear();
                symbolsValues.AddRange(value);
            }
        }

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
