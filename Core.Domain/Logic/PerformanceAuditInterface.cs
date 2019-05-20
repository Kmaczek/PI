using Core.Model.PerformanceCounterModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Domain.Logic
{
    public interface PerformanceAuditInterface
    {
        float GetCurrentCpuUsage();
        float GetAvailableRAM();
        List<ProcessUsage> GetProcessesUsages();
    }
}
