using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using Core.Model.FlatsModels;
using HtmlAgilityPack;

namespace Flats.Core.Scraping
{
    public delegate void ScrapedPageDelegate(IEnumerable<FlatDataBM> parsedFlats);

    public abstract class Scraper
    {
        public event ScrapedPageDelegate OnScrapedPage;

        public string ScrapingUrl { get; set; }

        public virtual string Name => "Generic";

        public List<string> Errors => new List<string>();

        protected abstract FlatDataBM ParseOffer(HtmlNode node);

        protected abstract HtmlNodeCollection GetOffers(HtmlDocument document);

        protected abstract int GetPageCount(HtmlDocument document);

        public IEnumerable<FlatDataBM> Scrape()
        {
            Errors.Clear();

            if (string.IsNullOrEmpty(ScrapingUrl))
            {
                throw new ArgumentException("ScrapingUrl is not set");
            }

            var results = new List<FlatDataBM>();
            var currentPage = 1;
            var pageContent = GetContent(String.Format(ScrapingUrl, currentPage));
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(pageContent);
            var pageCount = GetPageCount(document);
            results.AddRange(ScrapPage(document));
            OnScrapedPage(results);

            for (int i = 2; i <= pageCount; i++)
            {
                var content = GetContent(String.Format(ScrapingUrl, i));
                document.LoadHtml(content);
                var batch = ScrapPage(document);
                OnScrapedPage(batch);
                results.AddRange(batch);
                Thread.Sleep(600);
            }

            return results;
        }

        protected string GetContent(string url)
        {
            using (WebClient client = new WebClient())
            {
                client.Encoding = new UTF8Encoding();
                return client.DownloadString(new Uri(url));
            }
        }

        protected IEnumerable<FlatDataBM> ScrapPage(HtmlDocument document)
        {
            var parsedOffers = new List<FlatDataBM>();
            var offers = GetOffers(document);

            foreach (var offer in offers)
            {
                var parsedOffer = ParseOffer(offer);
                if (parsedOffer == null)
                {
                    continue;
                }
                parsedOffers.Add(parsedOffer);
            }

            return parsedOffers;
        }
    }

    public interface IScrapper
    {
        event ScrapedPageDelegate OnScrapedPage;
        string Name { get; }
        List<string> Errors { get; }
        string ScrapingUrl { get; set; }
        IEnumerable<FlatDataBM> Scrape();
    }
}
