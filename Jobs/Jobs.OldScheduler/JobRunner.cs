using Autofac;
using Core.Common;
using Core.Common.Logging;
using Jobs.OldScheduler.Jobs;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace Jobs.OldScheduler
{
    public class JobRunner
    {
        private readonly IContainer container;
        private readonly IConfigurationRoot configuration;

        public ILogger Log { get; }

        private List<string> jobsToStart = new List<string>();
        private static List<Timer> runningJobs = new List<Timer>();

        public JobRunner(
            IContainer container)
        {
            this.container = container;
            this.configuration = container.Resolve<IConfigurationRoot>();
            this.Log = new Log4Net(typeof(JobRunner));
        }

        public void AddJob(string jobName)
        {
            jobsToStart.Add(jobName);
        }

        public void RunJobs()
        {
            foreach (var jobName in jobsToStart)
            {
                var hour = Convert.ToInt32(configuration.GetSection($"{jobName}:hour").Value, CultureInfo.InvariantCulture);
                var minute = Convert.ToInt32(configuration.GetSection($"{jobName}:minute").Value, CultureInfo.InvariantCulture);
                var interval = configuration.GetSection($"{jobName}:interval").Value;
                var JobContext = new JobContext()
                {
                    Container = container,
                    JobName = jobName
                };

                if (interval == "daily")
                {
                    var configDate = DateTime.Now.Date + new TimeSpan(hour, minute, 0);
                    Log.Info($"{jobName} will run daily at: {configDate.ToShortTimeString()}");

                    var date = DateTime.Now.AddDays(1);
                    TimeSpan startTimeSpan;
                    if (configDate > DateTime.Now)
                    {
                        startTimeSpan = new TimeSpan(configDate.Ticks - DateTime.Now.Ticks);
                    }
                    else
                    {
                        startTimeSpan = new TimeSpan(configDate.AddDays(1).Ticks - DateTime.Now.Ticks);
                    }

                    var periodTimeSpan = TimeSpan.FromDays(1);

                    var timer = new Timer((c) =>
                    {
                        var context = JobContext as JobContext;
                        using (var scope = context.Container.BeginLifetimeScope())
                        {
                            var job = context.Container.ResolveNamed<IJob>(context.JobName);
                            job.Run();
                        }

                    }, JobContext, startTimeSpan, periodTimeSpan);

                    runningJobs.Add(timer);
                }
                else if (interval == "timeSpan")
                {
                    var timespan = new TimeSpan(hour, minute, 0);
                    Log.Info($"{jobName} will run every {timespan.Minutes} minutes.");

                    TimeSpan periodTimeSpan = new TimeSpan(hour, minute, 0);

                    var timer = new Timer((c) =>
                    {
                        var context = JobContext as JobContext;
                        using (var scope = context.Container.BeginLifetimeScope())
                        {
                            var job = context.Container.ResolveNamed<IJob>(context.JobName);
                            job.Run();
                        }

                    }, JobContext, TimeSpan.FromSeconds(1), periodTimeSpan);

                    runningJobs.Add(timer);
                }
                else if (interval == "never")
                {
                    Log.Info($"{jobName} configured not to run.");
                }
            }
        }

        private class JobContext
        {
            public IContainer Container { get; set; }
            public string JobName { get; set; }
        }
    }
}
