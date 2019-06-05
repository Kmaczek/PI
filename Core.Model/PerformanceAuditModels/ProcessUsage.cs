using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model.PerformanceAuditModels
{
    public class ProcessUsage
    {
        public int Pid { get; set; }
        public float Usage { get; set; }
        public string ProcessName { get; set; }
        public string ServiceName { get; set; }
    }
}
