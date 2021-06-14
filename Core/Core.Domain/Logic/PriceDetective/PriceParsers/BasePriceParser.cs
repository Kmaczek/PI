using Core.Model.PriceDetectiveModels;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;

namespace Core.Domain.Logic.PriceDetective.PriceParsers
{
    public abstract class BasePriceParser: IPriceParser
    {
        public string Content { get; set; } = String.Empty;
        public HtmlDocument HtmlDocument { get; set; } = new HtmlDocument();

        public virtual void Load(Uri uri)
        {
            Content = DownloadContent(uri);
            HtmlDocument.LoadHtml(Content);
        }

        public abstract PriceParserResult Parse();

        protected virtual string DownloadContent(Uri uri)
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

        protected virtual string GetPropertyValue(string selector)
        {
            var element = HtmlDocument.DocumentNode.SelectSingleNode(selector);
            var value = element.Attributes.FirstOrDefault(x => x.Name == "content").Value;
            return value;
        }
    }
}
