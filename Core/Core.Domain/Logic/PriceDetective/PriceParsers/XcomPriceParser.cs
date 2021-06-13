using Core.Common;
using Core.Model.PriceDetectiveModels;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;

namespace Core.Domain.Logic.PriceDetective.PriceParsers
{
    public class XcomPriceParser : IPriceParser
    {
        public string Content { get; set; } = String.Empty;
        public HtmlDocument HtmlDocument { get; set; } = new HtmlDocument();

        public XcomPriceParser()
        {
        }

        public void Load(Uri uri)
        {
            Content = DownloadContent(uri);
            HtmlDocument.LoadHtml(Content);
        }

        public PriceParserResult Parse()
        {
            if (string.IsNullOrEmpty(Content)) throw new Exception("No content loaded, call Load first.");
            var result = new PriceParserResult();
            try
            {
                var title = GetPropertyValue(@"//*[contains(@property,'og:title')]")?.Trim();
                var retailerPartNo = GetPropertyValue(@"//*[contains(@property,'product:retailer_part_no')]")?.Trim();
                var price = GetPropertyValue(@"//*[contains(@property,'product:price:amount')]")?.Trim();
                var priceNumber = Convert.ToDecimal(price, CultureInfo.InvariantCulture);

                result.Price = priceNumber;
                result.RetailerNo = retailerPartNo;
                result.Title = title;
                result.Proper = priceNumber != 0;
            }
            catch (Exception e)
            {
                result.Proper = false;
                throw;
            }

            return result;
        }

        protected string DownloadContent(Uri uri)
        {
            using (WebClient client = new WebClient())
            {
                client.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:89.0) Gecko/20100101 Firefox/89.0");
                client.Headers.Add(HttpRequestHeader.Referer, "https://www.x-kom.pl/szukaj?q=rtx");
                client.Headers.Add(HttpRequestHeader.Pragma, "no-cache");
                client.Headers.Add(HttpRequestHeader.Host, "www.x-kom.pl");
                //client.Headers.Add(HttpRequestHeader.Cookie, "gcCobrowseStatus=null; PHPSESSID=sb2mh8iej0jj2t7co0lk80na9i; gcCobrowseStatus=null; utm_source=duckduckgo.com; utm_medium=referral; recently_viewed=[%22507702%22]");
                client.Headers.Add(HttpRequestHeader.CacheControl, "no-cache");
                client.Encoding = new UTF8Encoding();
                return client.DownloadString(uri);
            }
        }

        private string GetPropertyValue(string selector)
        {
            var element = HtmlDocument.DocumentNode.SelectSingleNode(selector);
            var value = element.Attributes.FirstOrDefault(x => x.Name == "content").Value;
            return value;
        }
    }
}
