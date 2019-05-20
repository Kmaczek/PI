using Core.Common;
using Core.Model.PerformanceCounterModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Core.Domain.Logic
{
    public class PerformanceAudit : PerformanceAuditInterface
    {
        PerformanceCounter cpuCounter;
        PerformanceCounter ramCounter;
        private readonly LoggerInterface _log;

        public PerformanceAudit(
            LoggerInterface log
            )
        {
            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            ramCounter = new PerformanceCounter("Memory", "Available MBytes");
            
            this._log = log;
        }

        public float GetCurrentCpuUsage()
        {
            cpuCounter.NextValue();
            Thread.Sleep(500);
            return cpuCounter.NextValue();
        }

        public float GetAvailableRAM()
        {
            return ramCounter.NextValue();
        }

        public List<ProcessUsage> GetProcessesUsages()
        {
            var counters = new List<PerformanceCounter>();
            var processesUsage = new List<ProcessUsage>();

            try
            {
                var processes = GetProcessNames();

                foreach (var process in processes)
                {
                    var processCpu = new PerformanceCounter("Process", "% Processor Time", process, true);
                    processCpu.NextValue();
                    counters.Add(processCpu);
                }

                Thread.Sleep(1000);

                foreach (var counter in counters)
                {
                    try
                    {
                        processesUsage.Add(new ProcessUsage { ProcessName = counter.InstanceName, Usage = counter.NextValue() });
                    }
                    catch (InvalidOperationException e)
                    {
                        // Skip stopped process
                    }
                }
            }
            finally
            {
                counters.ForEach(x => x.Close());
            }

            return processesUsage;
        }

        private List<string> GetProcessNames()
        {
            var processesNames = new PerformanceCounterCategory("Process").GetInstanceNames().GroupBy(g => g);
            List<string> pcList = new List<string>();
            foreach (var pg in processesNames)
            {
                if (pg.Count() == 1)
                {
                    pcList.Add(pg.First());
                }
                else
                {
                    int id = 1;
                    foreach (var p in pg)
                    {
                        pcList.Add(p + "#" + id);
                        id++;
                    }
                }
            }

            return pcList;
        }
    }
}
