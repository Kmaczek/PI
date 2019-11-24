using Core.Model.BinanceModels;
using System.Globalization;
using System.Linq;

namespace Core.Domain.Logic.EmailGeneration
{
    public class BinanceHtmlGenerator : HtmlBase, IHtmlGenerator
    {
        private BinanceVM binanceModel;

        public string HtmlKey => "binance_body";

        public override string HtmlTemplateName => "Core.Domain.Logic.EmailGeneration.BinanceTemplate.html";

        public BinanceHtmlGenerator()
        {
        }

        public string GenerateBody()
        {
            if (binanceModel == null)
            {
                return "Brak danych";
            }

            return CombineHtmlWithData();
        }

        public void SetBodyData(object binanceVM)
        {
            this.binanceModel = binanceVM as BinanceVM;

            if (binanceModel != null)
            {
                DataDictionary.Add("symbol_value", binanceModel.SymbolsValues.Where(x => x.ConvertedPrice > 10));
                DataDictionary.Add("total_value", binanceModel.TotalValue.ToString("C2", CultureInfo.CreateSpecificCulture("en-US")));
            }
        }
    }
}
