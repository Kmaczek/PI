using CenyMieszkan.Models.FlatData;
using CenyMieszkan.Scraping;
using Core.Model.FlatsModels;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flats.Core.Scraping
{
    public class GratkaScrapper : Scraper
    {
        private string url = "https://gratka.pl/nieruchomosci/mieszkania/wroclaw/sprzedaz";
        protected override HtmlNodeCollection GetOffers(HtmlDocument document)
        {
            throw new NotImplementedException();
        }

        protected override int GetPageCount(HtmlDocument document)
        {
            throw new NotImplementedException();
        }

        protected override FlatDataBM ParseOffer(HtmlNode node)
        {
            throw new NotImplementedException();
        }
    }
}
