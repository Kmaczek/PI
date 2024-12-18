namespace Monitoring.Configs
{
    public class NginxConfig
    {
        public string ServiceName { get; set; } = "nginx";
        public string ExecutablePath { get; set; } = "nginx";
        public string ConfigPath { get; set; } = "";
        public int StartupDelaySeconds { get; set; } = 5;
    }
}
