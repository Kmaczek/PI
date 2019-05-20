using Core.Common;
using Core.Domain.Logic;
using Core.Model.PerformanceCounterModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Jobs.OldScheduler
{
    public class PerformanceAuditJob : JobInterface
    {
        public static Timer RunningTimer = null;

        private readonly IConfigurationRoot _configuration;
        private readonly LoggerInterface _log;
        private readonly PerformanceAuditInterface _performanceAudit;

        public PerformanceAuditJob(
            IConfigurationRoot configuration,
            LoggerInterface log,
            PerformanceAuditInterface performanceAudit)
        {
            this._configuration = configuration;
            this._log = log;
            this._performanceAudit = performanceAudit;
        }

        public void ImmediateRun()
        {
            AuditPerformance();
        }

        public void Run()
        {
            var jobInterval = Convert.ToInt32(_configuration.GetSection("performanceAudit:minuteInterval").Value);
            _log.Info($"Config for {nameof(PerformanceAuditJob)}. Job will run every: {jobInterval} minute(s).");

            var periodTimeSpan = TimeSpan.FromMinutes(jobInterval);

            RunningTimer = new Timer((e) =>
            {
                AuditPerformance();
            }, null, TimeSpan.Zero, periodTimeSpan);
            _log.Info($"{nameof(PerformanceAuditJob)} enqueued.");
        }
        
        private void AuditPerformance()
        {
            _log.Info("Monitoring machine performance...");
            _log.Info($"RAM {_performanceAudit.GetAvailableRAM()}MB");
            _log.Info($"CPU {_performanceAudit.GetCurrentCpuUsage()}%");
            var processesUsages = _performanceAudit.GetProcessesUsages();
            _log.Info($"Found {processesUsages.Count} processes.");
            LogProcesUsageData(processesUsages);
            _log.Info($"{nameof(PerformanceAuditJob)} finished.");
        }

        private void LogProcesUsageData(List<ProcessUsage> processesUsages)
        {
            var topTenUsages = processesUsages.OrderByDescending(x => x.Usage).Take(11);
            var total = topTenUsages.FirstOrDefault(x => x.ProcessName == "_Total");
            if(total == null || total.Usage == 0)
            {
                _log.Info("Total usage not found");
            }

            var sb = new StringBuilder("Displaying top 10 processes:");
            sb.AppendLine();
            foreach(var processUsage in topTenUsages.Skip(1))
            {
                var usage = Math.Round(processUsage.Usage / total.Usage * 100, 0).ToString().PadLeft(2, '0');
                sb.AppendLine($"{usage:0}% - {processUsage.ProcessName}");
            }
            _log.Info(sb.ToString());
        }
    }
}
