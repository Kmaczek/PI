using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Jobs.OldScheduler.HealthChecks
{
    public class SchedulerHealthCheck : IHealthCheck
    {
        public SchedulerHealthCheck()
        {
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var jobs = JobRunner.GetRunningJobs();

                if (jobs.Length < 1)
                    return Task.FromResult(HealthCheckResult.Unhealthy("Scheduler is not running any jobs"));

                var jobRuns = JobRunner.GetLastJobRuns();

                foreach (var job in jobs)
                {
                    if (job.ExecutionType == "daily" && job.ScheduledAt.AddDays(-1) > DateTime.UtcNow)
                    {
                        if (jobRuns.TryGetValue(job.JobName, out var lastRun) && lastRun > DateTime.UtcNow.AddDays(-1))
                        {
                            // everything ok
                        }
                        else
                        {
                            // job not run or we have some old run (3 days ago)
                            return Task.FromResult(HealthCheckResult.Unhealthy($"Scheduler did not run job {job.JobName}"));
                        }
                    }
                }

                return Task.FromResult(HealthCheckResult.Healthy("Scheduler is healthy"));
            }
            catch (Exception ex)
            {
                return Task.FromResult(HealthCheckResult.Unhealthy("Scheduler is unhealthy", ex));
            }
        }
    }
}
