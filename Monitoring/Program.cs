using Monitoring.Configs;
using System.Reflection;
using log4net;
using log4net.Config;

namespace Monitoring
{
    public class Program
    {
        public static void Main(string[] args)
        {
            SetupLogger();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
            .ConfigureLogging((con, logging) =>
            {
                logging.AddLog4Net();
                logging.SetMinimumLevel(LogLevel.Information);
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.Configure<AppConfiguration>(
                    hostContext.Configuration.GetSection("AppMonitor"));
                services.AddHostedService<MonitorService>();
                services.AddHttpClient();
            })
            .UseWindowsService(options =>
            {
                options.ServiceName = "PiMonitorService";
            });

        private static void SetupLogger()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            var configFile = new FileInfo(Path.Combine(basePath, "log4net.config"));

            if (!configFile.Exists)
            {
                throw new FileNotFoundException("log4net.config not found", configFile.FullName);
            }

            XmlConfigurator.Configure(logRepository, configFile);
            Console.WriteLine("Logging initialized successfully");
        }
    }
}