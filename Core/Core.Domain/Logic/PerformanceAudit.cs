using Core.Common;
using Core.Model.PerformanceAuditModels;
using Core.Model.PerformanceAuditModels.Exceptions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;

namespace Core.Domain.Logic
{
    public class PerformanceAudit : IPerformanceAudit
    {
        private const string AuditCounterSleepPeriod = "PerformanceAuditJob:counterSleepPeriod";

        private readonly ILogger log;
        private readonly IConfigurationRoot configuration;

        public PerformanceAudit(ILogger log, IConfigurationRoot configuration)
        {
            this.log = log;
            this.configuration = configuration;

            var sleepPeriod = configuration.GetSection(AuditCounterSleepPeriod).Value;

            if (string.IsNullOrEmpty(sleepPeriod))
                throw new PerformanceAuditException($"Missing configuration for PerformanceAudit service[{AuditCounterSleepPeriod}]");
            else
                SleepPeriod = Convert.ToInt32(sleepPeriod.Length, CultureInfo.InvariantCulture);

        }

        public int SleepPeriod { get; }

        public float GetCurrentCpuUsage()
        {
            using(var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total"))
            {
                cpuCounter.NextValue();
                Thread.Sleep(500);
                return cpuCounter.NextValue();
            }
        }

        public float GetAvailableRAM()
        {
            using(var ramCounter = new PerformanceCounter("Memory", "Available MBytes"))
            {
                return ramCounter.NextValue();
            }
        }

        public List<ProcessUsage> GetProcessesUsages()
        {
            var processesUsage = new List<ProcessUsage>();
            var counterUsagePairs = new List<Tuple<ProcessUsage, PerformanceCounter>>();

            try
            {
                processesUsage.AddRange(GetProcessNames());

                foreach (var process in processesUsage)
                {
                    using (var processCpu = new PerformanceCounter("Process", "% Processor Time", process.ProcessName, true))
                    {
                        try
                        {
                            processCpu.NextValue();
                            counterUsagePairs.Add(new Tuple<ProcessUsage, PerformanceCounter>(process, processCpu));
                        }
                        catch (InvalidOperationException)
                        {
                            //
                        }
                    }
                }

                Thread.Sleep(SleepPeriod);

                foreach (var counterUsagePair in counterUsagePairs)
                {
                    try
                    {
                        counterUsagePair.Item1.Usage = counterUsagePair.Item2.NextValue();
                    }
                    catch (InvalidOperationException)
                    {
                        // Skip stopped process
                    }
                }

                SetServiceNames(processesUsage);
            }
            finally
            {
                counterUsagePairs.ForEach(x => x.Item2.Close());
            }

            return processesUsage;
        }

        public static void SetServiceNames(List<ProcessUsage> usages)
        {
            foreach(var usage in usages.Where(x => x.ProcessName.Contains("svc")))
            {
                var query = "SELECT * FROM Win32_Service where ProcessId = " + usage.Pid;
                using (var searcher = new ManagementObjectSearcher(query))
                {
                    foreach (ManagementObject queryObj in searcher.Get())
                    {
                        usage.ServiceName = queryObj["Name"].ToString();
                    }
                }
            }
        }

        private List<ProcessUsage> GetProcessNames()
        {
            var processesNames = new PerformanceCounterCategory("Process").GetInstanceNames().Distinct();
            var processes = Process.GetProcesses(".")
                .Where(x => processesNames.Any(pn => pn == x.ProcessName))
                .GroupBy(g => g.ProcessName);
            List<ProcessUsage> pcList = new List<ProcessUsage>()
            {
                new ProcessUsage { ProcessName= "_Total" }
            };
            foreach (var pg in processes)
            {
                var pName = Process.GetProcessesByName(pg.Key);
                if (pg.Count() == 1)
                {
                    pcList.Add(new ProcessUsage
                    {
                        ProcessName = pg.First().ProcessName,
                        Pid = pg.First().Id
                    });
                }
                else
                {
                    int id = 1;
                    foreach (var p in pg)
                    {
                        pcList.Add(new ProcessUsage
                        {
                            ProcessName = p.ProcessName + "#" + id,
                            Pid = p.Id
                        });
                        id++;
                    }
                }
            }

            return pcList;
        }
    }
}
