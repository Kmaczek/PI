using Core.Model.FlatsModels;
using HtmlAgilityPack;
using System;

namespace Flats.Core.Scraping
{
    public class GratkaScrapper : Scraper
    {
        private readonly string url = "https://gratka.pl/nieruchomosci/mieszkania/wroclaw/sprzedaz";
        protected override HtmlNodeCollection GetOffers(HtmlDocument document)
        {
            throw new NotImplementedException();
        }

        protected override int GetPageCount(HtmlDocument document)
        {
            throw new NotImplementedException();
        }

        protected override FlatDataBm ParseOffer(HtmlNode node)
        {
            throw new NotImplementedException();
        }
    }
}
