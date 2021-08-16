using Core.Model.PriceDetectiveModels;
using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Core.Domain.Logic.PriceDetective.PriceParsers
{
    public class CastoramaPriceParser : IPriceParser
    {
        public string Content { get; set; } = String.Empty;

        private  CultureInfo _polishCulture = new CultureInfo("pl-PL");

        public void Load(Uri uri)
        {
            Content = DownloadContent(uri);
            CultureInfo ci = new CultureInfo("pl-PL");
            Thread.CurrentThread.CurrentCulture = ci;
        }

        public PriceParserResult Parse()
        {
            if (string.IsNullOrEmpty(Content)) throw new Exception("No content loaded, call Load first.");
            var result = new PriceParserResult();
            try
            {
                using (var sr = new StringReader(Content))
                {
                    var line = sr.ReadLine();
                    var startParsing = false;
                    var nameParsed = false;

                    while (!string.IsNullOrEmpty(line))
                    {
                        if (line.Contains("\"aggregateRating\":", StringComparison.InvariantCulture) 
                            || line.Contains("\"isRelatedTo\":", StringComparison.InvariantCulture))
                        {
                            break;
                        }

                        if (!startParsing && line.Contains("\"description\":", StringComparison.InvariantCulture))
                        {
                            startParsing = true;
                            line = sr.ReadLine();
                        }

                        if (startParsing)
                        {
                            
                            if (!nameParsed && line.Contains("\"name\":", StringComparison.InvariantCulture))
                            {
                                result.Title = GetValue(line);
                                nameParsed = true;
                            }
                            else if (line.Contains("\"gtin13\":", StringComparison.InvariantCulture)) 
                            {
                                result.ProductNo = GetValue(line);
                            }
                            else if (line.Contains("\"price\":", StringComparison.InvariantCulture)) 
                            {
                                var priceStr = GetValue(line).Replace(".", ",");
                                result.Price = Convert.ToDecimal(priceStr, _polishCulture);
                            }
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

            return result;
        }

        private string GetValue(string line)
        {
            var regex = new Regex(": \"(.+)\",");
            var match = regex.Match(line);
            var value = match.Groups[1].Value;

            return value;
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
    }
}
