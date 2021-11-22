using Microsoft.Extensions.Configuration;
using Core.Model.FlatsModels;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Core.Common;

namespace Flats.Core.Scraping
{
    public class OtoDomScrapper : Scraper, IScrapper
    {
        // price 100k to 500k, price per m 1750 to 8500
        public readonly string AllOffers = @"https://www.otodom.pl/sprzedaz/mieszkanie/wroclaw/?search%5Bfilter_float_price%3Afrom%5D=100000&search%5Bfilter_float_price%3Ato%5D=500000&search%5Bfilter_float_price_per_m%3Afrom%5D=1750&search%5Bfilter_float_price_per_m%3Ato%5D=8500&search%5Bdescription%5D=1&search%5Border%5D=filter_float_price%3Adesc&search%5Bdist%5D=0&search%5Bsubregion_id%5D=381&search%5Bcity_id%5D=39&nrAdsPerPage=72&page={0}";
        public readonly string PrivateOffers = "https://www.otodom.pl/sprzedaz/mieszkanie/wroclaw/?search%5Bfilter_float_price%3Afrom%5D=100000&search%5Bfilter_float_price%3Ato%5D=500000&search%5Bfilter_float_price_per_m%3Afrom%5D=1750&search%5Bfilter_float_price_per_m%3Ato%5D=8500&search%5Bdescription%5D=1&search%5Bprivate_business%5D=private&search%5Border%5D=filter_float_price%3Adesc&search%5Bdist%5D=0&search%5Bsubregion_id%5D=381&search%5Bcity_id%5D=39&nrAdsPerPage=72&page={0}";
        public readonly string TestingOffers = "https://www.otodom.pl/sprzedaz/mieszkanie/wroclaw/?search%5Bfilter_float_price%3Afrom%5D=350000&search%5Bfilter_float_price%3Ato%5D=400000&search%5Bfilter_float_price_per_m%3Afrom%5D=1750&search%5Bfilter_float_price_per_m%3Ato%5D=8500&search%5Bfilter_float_m%3Afrom%5D=40&search%5Bfilter_float_m%3Ato%5D=50&search%5Bfilter_enum_rooms_num%5D%5B0%5D=3&search%5Bdescription%5D=1&search%5Bprivate_business%5D=private&search%5Border%5D=filter_float_price%3Adesc&search%5Bcity_id%5D=39&search%5Bsubregion_id%5D=381&search%5Bregion_id%5D=1&nrAdsPerPage=72&page={0}";

        public OtoDomScrapper(
            IConfigurationRoot configuration,
            ILogger log)
        {
            AllOffers = configuration.GetSection("flats:otodomUrlAllOffers").Value;
            PrivateOffers = configuration.GetSection("flats:otodomUrlPrivateOffers").Value;

            ScrapingUrl = PrivateOffers;
            Log = log;
        }

        public override string Name => "OtoDom";
        public string Url { get; set; }
        public ILogger Log { get; }

        protected override int GetPageCount(HtmlDocument document)
        {
            var sth = document.DocumentNode.SelectSingleNode(@"//*[@data-cy='search.listing-panel.label.ads-number']/span[2]");

            var count = 0;
            if (sth == null)
            {
                count = 1;
            }
            else
            {
                var totalElements = int.Parse(sth.InnerText.Trim().Replace(" ", ""));
                count = Convert.ToInt32(Math.Ceiling(totalElements / 72d));
            }

            Log.Info($"Processing {count} pages.");
            return count;
        }

        protected override HtmlNodeCollection GetOffers(HtmlDocument document)
        {
            return document.DocumentNode.SelectNodes(@"//*[@data-cy='search.listing']/ul/li/a");
        }

        protected override FlatDataBm ParseOffer(HtmlNode node)
        {
            Url = GetUrl(node, Errors);

            //var locationNode = node.SelectSingleNode(@".//article/p[1]");
            var priceNode = node.SelectSingleNode(@".//article/p[2]");
            var detailsNode = node.SelectSingleNode(@".//article/p[3]");
            var bottomDetails = node.SelectSingleNode(@".//article/div[1]");
               
            var otodomId = GetOtoDomId(Errors);
            var totalPrice = GetPrice(priceNode, Errors);
            var rooms = GetRooms(detailsNode, Errors);
            var squareMeters = GetArea(detailsNode, Errors);
            var isPrivate = IsPrivateOffer(bottomDetails, Errors);
            //var location = GetLocation(locationNode, Errors);

            var data = new FlatDataBm(squareMeters, totalPrice, rooms, Url, isPrivate)
            {
                OtoDomId = otodomId,
                //Location = location
            };

            return data;
        }

        private decimal GetArea(HtmlNode node, List<string> errors)
        {
            decimal squareMeters = 0m;
            var normalized = string.Empty;

            try
            {
                var areaNode = node.SelectSingleNode(@".//span[2]");
                normalized = areaNode.InnerText.Trim();
                var str = normalized.Remove(normalized.Length - 3).Replace(" ", "").Replace(".", ",");

                squareMeters = decimal.Parse(str, CultureInfo.CreateSpecificCulture("pl-PL"));
            }
            catch (Exception e)
            {
                errors.Add($"Can't parse Area [{normalized}]. Offer: {Url}. Exception {e.Message}");
            }

            return squareMeters;
        }

        private string GetLocation(HtmlNode node, List<string> errors)
        {
            string location = null;

            try
            {
                var roomsNode = node.SelectSingleNode(@".//span[1]");
                location = roomsNode.InnerText.Trim();
            }
            catch (Exception e)
            {
                errors.Add($"Can't parse {nameof(location)}. Offer: {Url}. Exception {e.Message}");
            }

            return location;
        }

        private decimal GetPrice(HtmlNode node, List<string> errors)
        {
            decimal price = 0m;
            var normalized = string.Empty;

            try
            {
                normalized = node.InnerText.Trim();
                var str = normalized.Remove(normalized.Length - 3).Replace(" ", "").Replace(",", ".");

                price = decimal.Parse(str, CultureInfo.CreateSpecificCulture("pl-PL"));
            }
            catch (Exception e)
            {
                errors.Add($"Can't parse Price [{normalized}]. Offer: {Url}. Exception {e.Message}");
            }

            return price;
        }

        private int GetRooms(HtmlNode node, List<string> errors)
        {
            int rooms = 0;
            var normalized = string.Empty;

            try
            {
                var roomsNode = node.SelectSingleNode(@".//span[1]");
                normalized = roomsNode.InnerText.Trim().Split(' ')[0].Replace(">", "");

                rooms = int.Parse(normalized);
            }
            catch (Exception e)
            {
                errors.Add($"Can't parse Rooms [{normalized}]. Offer: {Url}. Exception {e.Message}");
            }

            return rooms;
        }

        private string GetUrl(HtmlNode node, List<string> errors)
        {
            string url = string.Empty;
            var normalized = string.Empty;

            try
            {
                url = node.GetAttributeValue("href", "");
            }
            catch (Exception e)
            {
                errors.Add($"Can't parse Url [{normalized}]. Exception {e.Message}");
            }

            return url;
        }

        private string GetOtoDomId(List<string> errors)
        {
            string otodomId = string.Empty;

            try
            {
                Regex regex = new Regex(@"(ID.+)");
                Match match = regex.Match(Url);

                if (match.Success)
                {
                    var groups = regex.GetGroupNames();
                    var lastOne = match.Groups.Count-1;
                    otodomId = match.Groups[lastOne].Value?.Replace("ID", "");
                }
            }
            catch (Exception e)
            {
                errors.Add($"Can't parse Url [{Url}]. Exception {e.Message}");
            }

            return otodomId;
        }

        private bool IsPrivateOffer(HtmlNode node, List<string> errors)
        {
            bool isPrivate = false;
            var normalized = string.Empty;

            try
            {
                //var selectedNode = node.SelectSingleNode(@"//*[@class='offer-item-details-bottom']/ul");

                isPrivate = node.InnerText.Contains("Oferta prywatna");
            }
            catch (Exception e)
            {
                errors.Add($"Can't parse Url [{normalized}]. Exception {e.Message}");
            }

            return isPrivate;
        }
    }
}
