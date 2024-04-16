using Core.Model.PriceDetectiveModels;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Core.Domain.Logic.PriceDetective.PriceParsers
{
    public class DozPriceParser : IPriceParser
    {
        public DozPriceParser()
        {

        }

        public string Content { get; set; } = String.Empty;
        public HtmlDocument HtmlDocument { get; set; } = new HtmlDocument();

        public void Load(Uri uri)
        {
            Content = DownloadContent(uri);
            HtmlDocument.LoadHtml(Content);
        }

        public IEnumerable<PriceParserResult> Parse()
        {
            if (string.IsNullOrEmpty(Content)) throw new Exception("No content loaded, call Load first.");
            var result = new PriceParserResult();
            try
            {
                var title = GetPropertyValue(@"//*[contains(@property,'og:title')]")?.Trim();
                var brand = GetPropertyValue(@"//*[contains(@property,'product:brand')]")?.Trim();
                var retailerPartNo = GetPropertyValue(@"//*[contains(@property,'product:retailer_part_no')]")?.Trim();

                var price = string.Empty;
                var priceNode = HtmlDocument.DocumentNode
                    .SelectSingleNode(@"//*/meta[contains(@itemprop,'price')]");
                if (priceNode != null)
                {
                    price = priceNode
                    .Attributes
                    .FirstOrDefault(a => a.Name == "content")
                    .Value;
                }

                if (string.IsNullOrEmpty(price))
                {
                    priceNode = HtmlDocument.DocumentNode.SelectSingleNode(@"//div[@class='product-card-price-box']/div[@class='price']/text()");
                    price = Regex.Match(priceNode.InnerHtml, @"(\d+(?:,\d+)?)").Groups[1].Value;
                }

                try
                {
                    var pricePerQuantNode = HtmlDocument.DocumentNode.SelectSingleNode(@"//div[@class='product-card-price-box']/div[@class='price']/div");
                    var pricePerQuant = pricePerQuantNode
                        .InnerHtml
                        .Replace("zł", string.Empty)
                        .Replace("Cena za szt.:", string.Empty);

                    var resultString = Regex.Match(pricePerQuantNode.InnerHtml, @"(\d+(?:,\d+)?)").Groups[1].Value;

                    Console.WriteLine($"Price Per Quantity: {pricePerQuant.Trim()} - {resultString} - {Convert.ToDecimal(resultString, CultureInfo.GetCultureInfo("pl"))}");
                }
                catch (Exception) { }

                var priceNumber = Convert.ToDecimal(price, CultureInfo.GetCultureInfo("pl"));

                result.Price = priceNumber;
                result.ProductNo = $"{brand ?? string.Empty}.{retailerPartNo}";
                result.Title = title;
                result.Proper = priceNumber != 0;
            }
            catch (Exception e)
            {
                result.Proper = false;
                throw;
            }

            return new List<PriceParserResult>() { result };
        }

        private string DownloadContent(Uri uri)
        {
            using (WebClient client = new WebClient())
            {
                client.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:89.0) Gecko/20100101 Firefox/89.0");
                //client.Headers.Add(HttpRequestHeader.Referer, "https://www.doz.pl/apteka");
                client.Headers.Add(HttpRequestHeader.Pragma, "no-cache");
                //client.Headers.Add(HttpRequestHeader.Host, "www.x-kom.pl");
                client.Headers.Add(HttpRequestHeader.CacheControl, "no-cache");
                client.Encoding = new UTF8Encoding();
                return client.DownloadString(uri);
            }
        }

        private string GetPropertyValue(string selector)
        {
            string result = null;
            var element = HtmlDocument.DocumentNode.SelectSingleNode(selector);

            if (element != null)
                result = element.Attributes.FirstOrDefault(x => x.Name == "content")?.Value;

            return result;
        }
    }
}
