namespace Core.Model.TwoMiners
{
    public class ConfigModel
    {
        public long AllowedMaxPayout { get; set; }
        public long AllowedMinPayout { get; set; }
        public long DefaultMinPayout { get; set; }
        public string IpHint { get; set; }
        public string IpWorkerName { get; set; }
        public long MinPayout { get; set; }
    }
}
