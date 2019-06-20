using Core.Model.PerformanceAuditModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Domain.Logic
{
    public interface IPerformanceAudit
    {
        float GetCurrentCpuUsage();
        float GetAvailableRAM();
        List<ProcessUsage> GetProcessesUsages();
    }
}
