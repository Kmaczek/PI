namespace Monitoring.Configs
{
    public class AppConfiguration
    {
        public List<ApplicationConfig> Applications { get; set; } = new();
        public int CheckIntervalSeconds { get; set; } = 30;
        public int ErrorIntervalSeconds { get; set; } = 3600;
        public NginxConfig? NginxConfig { get; set; }
    }
}
