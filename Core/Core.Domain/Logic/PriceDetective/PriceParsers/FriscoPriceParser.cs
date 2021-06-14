using Core.Model.PriceDetectiveModels;
using System;
using System.Globalization;
using System.Linq;

namespace Core.Domain.Logic.PriceDetective.PriceParsers
{
    public class FriscoPriceParser : BasePriceParser
    {
        public override PriceParserResult Parse()
        {
            if (string.IsNullOrEmpty(Content)) throw new Exception("No content loaded, call Load first.");
            var result = new PriceParserResult();
            try
            {
                string priceStr = String.Empty;
                try
                {
                    priceStr = HtmlDocument.DocumentNode.SelectSingleNode(@"//*[contains(@class,'product-page_short-desc')]/div[contains(., 'Cena')]/span[@class='value']").InnerText;
                }
                catch(Exception e)
                {
                    priceStr = HtmlDocument.DocumentNode.SelectSingleNode(@"//*[contains(@class,'main-price')]").InnerText;
                }
                var price = Convert.ToDecimal(priceStr.Replace("zł", String.Empty).Trim(), CultureInfo.GetCultureInfo("pl"));
                var ean = HtmlDocument.DocumentNode.SelectSingleNode(@"//*[contains(@class,'product-page_short-desc')]/div[contains(., 'EAN')]/span[@class='value']").InnerText;

                var title = HtmlDocument.DocumentNode.SelectSingleNode(@"//*/h1[contains(@itemprop,'name')]").InnerText?.Trim();
                
                result.Price = price;
                result.ProductNo = ean;
                result.Title = title;
                result.Proper = price != 0;
            }
            catch (Exception e)
            {
                result.Proper = false;
                throw;
            }

            return result;
        }
    }
}
