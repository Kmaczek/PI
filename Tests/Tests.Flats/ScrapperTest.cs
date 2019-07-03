using Core.Domain.Logic;
using Core.Model.FlatsModels;
using Flats.Core.Scraping;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.Flats
{
    public class ScrapperTest
    {
        public static string PricesWithComas = @"https://www.otodom.pl/sprzedaz/mieszkanie/wroclaw/?search%5Bfilter_float_price%3Afrom%5D=336200&search%5Bfilter_float_price%3Ato%5D=487200&search%5Bfilter_float_m%3Afrom%5D=4&search%5Bfilter_float_m%3Ato%5D=41&search%5Bfilter_enum_rooms_num%5D%5B0%5D=2&search%5Bfilter_enum_floor_no%5D%5B0%5D=floor_3&search%5Bdescription%5D=1&search%5Bdist%5D=0&search%5Bsubregion_id%5D=381&search%5Bcity_id%5D=39&nrAdsPerPage=48&Page=1";

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void TestComas()
        {
            //var otoDomScrapper = new OtoDomScrapper();
            //otoDomScrapper.ScrapingUrl = PricesWithComas;
            //var allScrapeResult = otoDomScrapper.Scrape();

            //var allFlatAggregate = new FlatAggregateVM(allScrapeResult);

            //Assert.Pass();
        }
    }
}
