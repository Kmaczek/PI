using Core.Model.PriceDetectiveModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace Core.Domain.Logic.PriceDetective.PriceParsers
{
    public class CastoramaPriceParser : IPriceParser
    {
        public string Content { get; set; } = String.Empty;

        public void Load(Uri uri)
        {
            Content = DownloadContent(uri);
            CultureInfo ci = new CultureInfo("pl-PL");
            Thread.CurrentThread.CurrentCulture = ci;
        }

        public IEnumerable<PriceParserResult> Parse()
        {
            if (string.IsNullOrEmpty(Content)) throw new Exception("No content loaded, call Load first.");
            var result = new PriceParserResult();
            try
            {
                using (var sr = new StringReader(Content))
                {
                    var line = sr.ReadLine();

                    while (!string.IsNullOrEmpty(line))
                    {
                        if (line.Contains("\"offers\":"))
                        {
                            var jsonString = ExtractJson(line);
                            var product = JsonConvert.DeserializeObject<Product>(jsonString);

                            result.Price = product.Offers.Price;
                            result.ProductNo = product.Gtin13;
                            result.Title = product.Name;
                            result.Proper = result.Price != 0;

                            break;
                        }

                        line = sr.ReadLine();
                    }
                }
                
                result.Proper = result.Price != 0;
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
                client.Headers.Add(HttpRequestHeader.Pragma, "no-cache");
                client.Headers.Add(HttpRequestHeader.CacheControl, "no-cache");
                client.Encoding = new UTF8Encoding();
                return client.DownloadString(uri);
            }
        }

        private string ExtractJson(string line)
        {
            var typeAppJson = "type=\"application/ld+json\">";
            var endTypeAppJson = "</script>";
            var workedString = line;

            while (workedString.IndexOf(typeAppJson) >= 0)
            {
                var startJsonIndex = workedString.IndexOf(typeAppJson) + typeAppJson.Length;
                var endJsonIndex = workedString.IndexOf(endTypeAppJson, startJsonIndex);
                var workedSubstring = workedString.Substring(startJsonIndex, endJsonIndex - startJsonIndex);

                if (workedSubstring.Contains("\"@type\":\"Product\""))
                {
                    return workedSubstring;
                }
                else
                {
                    workedString = workedString.Substring(endJsonIndex);
                }
            }

            return string.Empty;
        }

        public class Product
        {
            public string Description { get; set; }
            public string Name { get; set; }
            public string Image { get; set; }
            [JsonProperty("@type")]
            public string Type { get; set; }
            public string Category { get; set; }
            public string Sku { get; set; }
            public Brand Brand { get; set; }
            public string Gtin13 { get; set; }
            public Offer Offers { get; set; }
        }

        public class Brand
        {
            [JsonProperty("@type")]
            public string Type { get; set; }
            public string Name { get; set; }
        }

        public class Offer
        {
            public string PriceCurrency { get; set; }
            public string Url { get; set; }
            public string ItemCondition { get; set; }
            public string Availability { get; set; }
            public decimal Price { get; set; }
            public Seller Seller { get; set; }
        }

        public class Seller
        {
            [JsonProperty("@type")]
            public string Type { get; set; }
            public string Name { get; set; }
        }
    }
}
