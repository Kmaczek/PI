namespace Monitoring.Configs
{
    public class ApplicationConfig
    {
        public string Name { get; set; } = "";
        public string ProcessName { get; set; } = "";
        public string ExecutablePath { get; set; } = "";
        public string Arguments { get; set; } = "";
        public string? HealthEndpoint { get; set; }
        public bool IsRunning { get; set; }
        public DateTime LastRestart { get; set; }
        public bool IsNginxHosted { get; set; }
    }
}
