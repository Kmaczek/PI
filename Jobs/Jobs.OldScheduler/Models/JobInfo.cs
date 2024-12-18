using System;

namespace Jobs.OldScheduler.Models
{
    public record JobInfo(string JobName, string ExecutionType, TimeSpan ExecutionPeriod)
    {
        public DateTime ScheduledAt { get; } = DateTime.UtcNow;
    }
}
