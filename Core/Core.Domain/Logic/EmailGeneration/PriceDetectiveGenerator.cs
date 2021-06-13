using Core.Model.PriceDetectiveModels;
using System.Globalization;
using System.Linq;

namespace Core.Domain.Logic.EmailGeneration
{
    public class PriceDetectiveGenerator : HtmlBase, IHtmlGenerator
    {
        private PriceDetectiveEmailModel _priceDetectiveModel;

        public string HtmlKey => "price_detective_body";

        public override string HtmlTemplateName => "Core.Domain.Logic.EmailGeneration.PriceDetectiveTemplate.html";

        public PriceDetectiveGenerator()
        {
        }

        public string GenerateBody()
        {
            if (_priceDetectiveModel == null)
            {
                return "Brak danych";
            }

            return CombineHtmlWithData();
        }

        public void SetBodyData(object priceDetectiveModel)
        {
            _priceDetectiveModel = priceDetectiveModel as PriceDetectiveEmailModel;

            if (_priceDetectiveModel != null)
            {
                DataDictionary.Add("items", _priceDetectiveModel.Items);
            }
        }
    }
}
