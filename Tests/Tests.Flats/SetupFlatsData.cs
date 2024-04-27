using NUnit.Framework;

namespace Tests.Flats
{
    public class SetupFlatsData
    {
        public static string OtodomDataPath = @".\SerializedFlats\otodomData.json";

        [Test]
        public void SetupOtodomData()
        {
            //var otoDomScrapper = new OtoDomScrapper();
            //var scrapeResult = otoDomScrapper.Scrape();

            //var serializedFlats = JsonConvert.SerializeObject(scrapeResult);

            //File.WriteAllText(OtodomDataPath, serializedFlats);
        }
    }
}
