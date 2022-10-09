using Core.Common;
using Core.Domain.Logic;
using Core.Model.PerformanceAuditModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;

namespace Jobs.OldScheduler.Jobs
{
    public class PerformanceAuditJob : IJob
    {
        private static Timer RunningTimer = null;

        private readonly IConfigurationRoot _configuration;
        private readonly ILogger _log;
        private readonly IPerformanceAudit _performanceAudit;

        public PerformanceAuditJob(
            IConfigurationRoot configuration,
            ILogger log,
            IPerformanceAudit performanceAudit)
        {
            _configuration = configuration;
            _log = log;
            _performanceAudit = performanceAudit;
        }

        public string JobName => nameof(PerformanceAuditJob);

        public void ImmediateRun()
        {
            AuditPerformance();
        }

        public void ImmediateRun(IEnumerable<string> parameters)
        {
            throw new NotImplementedException();
        }

        public void Run()
        {
            AuditPerformance();
        }

        private void AuditPerformance()
        {
            try
            {
                _log.Info("Monitoring machine performance...");
                _log.Info($"RAM {_performanceAudit.GetAvailableRAM()}MB");
                _log.Info($"CPU {_performanceAudit.GetCurrentCpuUsage()}%");
                var processesUsages = _performanceAudit.GetProcessesUsages();
                _log.Info($"Found {processesUsages.Count} processes.");
                LogProcesUsageData(processesUsages);
                _log.Info($"{JobName} finished.");
            }
            catch(Exception e)
            {
                _log.Error($"Exception happened when trying to process otodom data.", e);
            }
        }

        private void LogProcesUsageData(List<ProcessUsage> processesUsages)
        {
            var topTenUsages = processesUsages.OrderByDescending(x => x.Usage).Take(11);
            var total = topTenUsages.FirstOrDefault(x => x.ProcessName == "_Total");
            if (total == null || total.Usage == 0)
            {
                _log.Info("Total usage not found");
            }

            var sb = new StringBuilder("Displaying top 10 processes:");
            sb.AppendLine();
            foreach (var processUsage in topTenUsages.Skip(1))
            {
                var usage = Math
                    .Round(processUsage.Usage / total.Usage * 100, 0, MidpointRounding.AwayFromZero)
                    .ToString(CultureInfo.InvariantCulture)
                    .PadLeft(2, '0');
                sb.AppendLine($"{usage:0}% - {processUsage.ProcessName}{(processUsage.ServiceName != null ? " | " : "")}{processUsage.ServiceName}");
            }
            _log.Info(sb.ToString());
        }
    }
}
