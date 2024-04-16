using Core.Model.PriceDetectiveModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Core.Domain.Logic.PriceDetective.PriceParsers
{
    public class FriscoPriceParser : BasePriceParser
    {
        public override IEnumerable<PriceParserResult> Parse()
        {
            if (string.IsNullOrEmpty(Content)) throw new Exception("No content loaded, call Load first.");
            var result = new PriceParserResult();
            try
            {
                string priceStr = String.Empty;
                try
                {
                    var priceNodes = HtmlDocument.DocumentNode.SelectNodes(@"//*[contains(@class,'new-product-page__prices_unit-price')]");

                    if (PriceIsInvalid(priceNodes))
                        priceNodes = HtmlDocument.DocumentNode.SelectNodes(@"//*[contains(@class,'product-page_short-desc')]/div[contains(., 'Cena')]/span[@class='value']");

                    if (PriceIsInvalid(priceNodes))
                        priceNodes = HtmlDocument.DocumentNode.SelectNodes(@"//*[contains(@class,'new-product-page__prices_price')]/div[contains(., 'Cena')]/strong");

                    if (PriceIsInvalid(priceNodes))
                        priceNodes = HtmlDocument.DocumentNode.SelectNodes(@"//*[contains(@class,'main-price')]");

                    if (PriceIsInvalid(priceNodes))
                    {
                        var part1 = HtmlDocument.DocumentNode.SelectSingleNode(@"//*[contains(@class,'new-product-page__prices_price')]/span/span[contains(@class, 'price_num')]");
                        var part2 = HtmlDocument.DocumentNode.SelectSingleNode(@"//*[contains(@class,'new-product-page__prices_price')]/span/span[contains(@class, 'price_decimals')]");

                        if (part1 != null || part2 != null)
                            priceStr = $"{part1?.InnerText},{part2?.InnerText}";
                    }

                    if (priceNodes != null && priceNodes.Count > 0)
                        priceStr = priceNodes[0].InnerText;
                }
                catch (Exception e)
                {
                    priceStr = "0";
                }

                var priceFound = Regex.Match(priceStr, @"(\d+(?:,\d+)?)").Groups[1].Value;
                var price = Convert.ToDecimal(priceFound.Trim(), CultureInfo.GetCultureInfo("pl"));

                string ean = String.Empty;
                try
                {
                    var eanNode = HtmlDocument.DocumentNode.SelectSingleNode(@"//*[contains(@class,'product-page_short-desc')]/div[contains(., 'EAN')]/span[@class='value']");

                    if (eanNode == null)
                        eanNode = HtmlDocument.DocumentNode.SelectSingleNode(@"//*[contains(@class,'new-product-page__parameters')]/div[contains(., 'EAN')]/strong");

                    if (eanNode != null)
                        ean = eanNode.InnerText;
                    else
                        result.Proper = false;
                }
                catch (Exception e)
                {
                    result.Proper = false;
                }

                string title = String.Empty;
                try
                {
                    var titleNode = HtmlDocument.DocumentNode.SelectSingleNode(@"//*/h1[contains(@itemprop,'name')]");

                    if (titleNode == null)
                        titleNode = HtmlDocument.DocumentNode.SelectSingleNode(@"//*[contains(@class,'large-title')]");

                    if (titleNode != null)
                        title = titleNode.InnerText?.Trim();
                    else
                        result.Proper = false;
                }
                catch (Exception e)
                {
                    result.Proper = false;
                }

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

            return new List<PriceParserResult>() { result }; ;
        }

        private static bool PriceIsInvalid(HtmlAgilityPack.HtmlNodeCollection priceNodes)
        {
            return priceNodes == null || priceNodes.Count < 1 || String.IsNullOrWhiteSpace(priceNodes[0].InnerText) || priceNodes[0].InnerText.Contains("0,00");
        }
    }
}
