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
    public class PerformanceAudit : IPerformanceAudit, IDisposable
    {
        private bool disposed = false;
        private PerformanceCounter cpuCounter;
        private PerformanceCounter ramCounter;
        private readonly ILogger log;

        public PerformanceAudit(ILogger log)
        {
            this.log = log;
        }

        public PerformanceCounter CpuCounter
        {
            get
            {
                if (cpuCounter == null)
                    cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");

                return cpuCounter;
            }
        }

        public PerformanceCounter RamCounter
        {
            get
            {
                if (cpuCounter == null)
                    cpuCounter = new PerformanceCounter("Memory", "Available MBytes");

                return cpuCounter;
            }
        }

        public float GetCurrentCpuUsage()
        {
            CpuCounter.NextValue();
            Thread.Sleep(500);
            return CpuCounter.NextValue();
        }

        public float GetAvailableRAM()
        {
            return RamCounter.NextValue();
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
                    catch (InvalidOperationException)
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                if (cpuCounter != null)
                {
                    cpuCounter.Dispose();
                    cpuCounter = null;
                }

                if (ramCounter != null)
                {
                    ramCounter.Dispose();
                    ramCounter = null;
                }

                disposed = true;
            }
        }
    }
}
