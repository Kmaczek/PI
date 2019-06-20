using Core.Model.BinanceModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Core.Domain.Logic.EmailGeneration
{
    public class BinanceHtmlGenerator : HtmlBase, IHtmlGenerator
    {
        private readonly BinanceVM binanceModel;

        public string HtmlKey => "binance_body";

        public override string HtmlTemplateName => "Core.Domain.Logic.EmailGeneration.BinanceTemplate.html";

        public BinanceHtmlGenerator(BinanceVM binanceVM)
        {
            this.binanceModel = binanceVM;

            DataDictionary.Add("symbol_value", binanceModel.SymbolsValues);
            DataDictionary.Add("total_value", binanceModel.TotalValue.ToString("C2", CultureInfo.CreateSpecificCulture("en-US")));
        }

        public string GenerateBody()
        {
            if (binanceModel == null)
            {
                return "Brak danych";
            }

            return CombineHtmlWithData();
        }
    }
}
