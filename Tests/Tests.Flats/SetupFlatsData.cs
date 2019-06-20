using Flats.Core.Scraping;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Tests.Flats
{
    public class SetupFlatsData
    {
        public static string OtodomDataPath = @".\SerializedFlats\otodomData.json";

        [Test]
        public void SetupOtodomData()
        {
            var otoDomScrapper = new OtoDomScrapper();
            var scrapeResult = otoDomScrapper.Scrape();

            var serializedFlats = JsonConvert.SerializeObject(scrapeResult);

            File.WriteAllText(OtodomDataPath, serializedFlats);
        }
    }
}
