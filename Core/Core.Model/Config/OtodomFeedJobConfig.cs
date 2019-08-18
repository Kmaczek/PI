using Newtonsoft.Json;

namespace Core.Model.Config
{
    public class OtodomFeedJobConfig
    {
        [JsonProperty("hour")]
        public int Hour { get; private set; }

        [JsonProperty("minute")]
        public int Minute { get; private set; }

        [JsonProperty("interval")]
        public string Interval { get; private set; }

        [JsonProperty("allOffersUrl")]
        public string AllOffersUrl { get; private set; }

        [JsonProperty("testingUrl")]
        public string TestingUrl { get; private set; }
    }
}
