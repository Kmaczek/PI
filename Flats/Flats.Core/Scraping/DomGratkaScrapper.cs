using System;
using CenyMieszkan.Models.FlatData;
using Core.Model.FlatsModels;
using HtmlAgilityPack;

namespace Flats.Core.Scraping
{
    public class DomGratkaScrapper : Scraper
    {
        private string privateUrl = @"http://dom.gratka.pl/mieszkania-sprzedam/lista/,wroclaw,40,{0},on,li,s,zi.html";

        public DomGratkaScrapper()
        {
            ScrapingUrl = privateUrl;
        }

        public override string Name => "DomGratka";

        protected override HtmlNodeCollection GetOffers(HtmlDocument document)
        {
            return document.DocumentNode.SelectNodes(@"//*[@data-gtm='zajawka']");
        }

        protected override int GetPageCount(HtmlDocument document)
        {
            var sth = document.DocumentNode.SelectSingleNode(@"//*[@class='strona']");
            return int.Parse(sth.InnerText.Trim());
        }

        protected override FlatDataBM ParseOffer(HtmlNode node)
        {
            var ogloszenieInfo = node.SelectSingleNode(@".//*[@class='ogloszenieInfo']");
            FlatDataBM data;

            try
            {
                var Url = GetUrl(node);
                //var Location = GetLocation(ogloszenieInfo);
                var Rooms = GetRooms(ogloszenieInfo);
                var TotalPrice = GetPrice(node);
                var SquareMeters = GetSquareMeters(ogloszenieInfo);
                //var Year = GetYear(ogloszenieInfo);

                data = new FlatDataBM(SquareMeters, TotalPrice, Rooms, Url, false);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Skipped: {ogloszenieInfo?.InnerHtml ?? "No inner HTML"} \n" + e);
                return null;
            }

            return data;
        }

        private int GetYear(HtmlNode ogloszenieInfo)
        {
            var urlNode = ogloszenieInfo.SelectSingleNode(@".//div/p/span[3]");
            return int.Parse(urlNode.InnerText);
        }

        private decimal GetSquareMeters(HtmlNode ogloszenieInfo)
        {
            var urlNode = ogloszenieInfo.SelectSingleNode(@"(.//div/p/span)[last()]/b");
            return decimal.Parse(urlNode.InnerText);
        }

        private decimal GetPrice(HtmlNode node)
        {
            var urlNode = node.SelectSingleNode(@".//*[@class='detailedPrice']/p/b");
            return decimal.Parse(urlNode.InnerText);
        }

        private int GetRooms(HtmlNode ogloszenieInfo)
        {
            var urlNode = ogloszenieInfo.SelectSingleNode(@".//div/p/span[1]");
            return int.Parse(urlNode.InnerText);
        }

        private string GetLocation(HtmlNode node)
        {
            var locationNode = node.SelectSingleNode(@".//h2");
            return locationNode.InnerText;
        }

        private string GetUrl(HtmlNode node)
        {
            var urlNode = node.SelectSingleNode(@".//a");
            return "http://dom.gratka.pl" + urlNode.GetAttributeValue("href", "");
        }
    }
}
