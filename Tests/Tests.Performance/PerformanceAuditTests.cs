using Core.Common.Logging;
using Core.Domain.Logic;
using NUnit.Framework;
using System.Linq;

namespace Tests.Performance
{
    public class PerformanceAuditTests
    {
        public static ILogger Log = new Log4Net(typeof(PerformanceAuditTests));

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            //var performanceAudit = new PerformanceAudit(Log, );

            //var currentCpu = performanceAudit.GetCurrentCpuUsage();
            //var currentRam = performanceAudit.GetAvailableRAM();

            //var usage = performanceAudit.GetProcessesUsages();
            //var total = usage.FirstOrDefault(x => x.ProcessName == "_Total");

            Assert.Pass();
        }
    }
}