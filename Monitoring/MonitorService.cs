using Microsoft.Extensions.Options;
using Monitoring.Configs;
using System.Diagnostics;

namespace Monitoring
{
    public class MonitorService : BackgroundService
    {
        private readonly ILogger<MonitorService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly AppConfiguration _config;

        public MonitorService(ILogger<MonitorService> logger, IHttpClientFactory httpClientFactory, IOptions<AppConfiguration> config)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _config = config.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting to monitor applications.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Monitoring applications in loop...");
                    if (_config.NginxConfig != null)
                    {
                        await MonitorNginx();
                    }

                    foreach (var app in _config.Applications)
                    {
                        await MonitorApplication(app);
                    }

                    await Task.Delay(TimeSpan.FromSeconds(_config.CheckIntervalSeconds), stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while monitoring applications");
                    await Task.Delay(TimeSpan.FromSeconds(_config.ErrorIntervalSeconds), stoppingToken);
                }
            }
        }

        private async Task MonitorNginx()
        {
            try
            {
                if (_config.NginxConfig == null) return;

                var isRunning = IsProcessRunning(_config.NginxConfig.ServiceName);
                if (!isRunning)
                {
                    await RestartNginx();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error monitoring Nginx");
            }
        }

        private async Task RestartNginx()
        {
            try
            {
                if (_config.NginxConfig == null) return;
                _logger.LogWarning("Restarting Nginx...");

                // Kill existing Nginx processes
                foreach (var process in Process.GetProcessesByName(_config.NginxConfig.ServiceName))
                {
                    try
                    {
                        process.Kill();
                        await Task.Delay(2000);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error killing Nginx process");
                    }
                }

                // Start Nginx
                var startInfo = new ProcessStartInfo
                {
                    FileName = _config.NginxConfig.ExecutablePath,
                    Arguments = $"-s reload",
                    UseShellExecute = true,
                    WindowStyle = ProcessWindowStyle.Normal
                };

                using var newProcess = Process.Start(startInfo);
                _logger.LogInformation("Nginx restarted successfully");
                await Task.Delay(TimeSpan.FromSeconds(_config.NginxConfig.StartupDelaySeconds));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error restarting Nginx");
            }
        }

        private async Task MonitorApplication(ApplicationConfig app)
        {
            try
            {
                var isRunning = app.IsNginxHosted || IsProcessRunning(app.ProcessName);
                var isHealthy = true;

                if (isRunning && !string.IsNullOrEmpty(app.HealthEndpoint))
                {
                    isHealthy = await CheckHealthEndpoint(app.HealthEndpoint);
                }

                if (!isRunning || !isHealthy)
                {
                    if (app.IsNginxHosted)
                    {
                        // For Nginx-hosted apps, only restart if health check fails
                        if (!isHealthy)
                        {
                            await RestartNginx();
                        }
                    }
                    else
                    {
                        await RestartApplication(app);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error monitoring {app.Name}");
            }
        }

        private bool IsProcessRunning(string processName)
        {
            return Process.GetProcessesByName(processName).Length > 0;
        }

        private async Task<bool> CheckHealthEndpoint(string healthEndpoint)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync(healthEndpoint);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        private async Task RestartApplication(ApplicationConfig app)
        {
            try
            {
                _logger.LogWarning($"Restarting {app.Name}...");

                // Kill existing process if it's still running
                foreach (var process in Process.GetProcessesByName(app.ProcessName))
                {
                    try
                    {
                        process.Kill();
                        await Task.Delay(2000); // Wait for process to terminate
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error killing process {app.ProcessName}");
                    }
                }

                // Start the application
                var startInfo = new ProcessStartInfo
                {
                    FileName = app.ExecutablePath,
                    Arguments = app.Arguments,
                    UseShellExecute = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                };

                if (app.ProcessName.Equals("node", StringComparison.OrdinalIgnoreCase))
                {
                    // Special handling for Angular application
                    startInfo.FileName = "cmd.exe";
                    startInfo.Arguments = $"/c cd {app.ExecutablePath} && npm start";
                }

                using var newProcess = Process.Start(startInfo);
                app.LastRestart = DateTime.Now;
                _logger.LogInformation($"{app.Name} restarted successfully");

                // Wait for the application to initialize
                await Task.Delay(10000);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error restarting {app.Name}");
            }
        }
    }
}
