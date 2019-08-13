using Core.Common;
using Core.Domain.Logic.OtodomService;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading;

namespace Jobs.OldScheduler.Jobs
{
    public class OtodomFeedJob : IJob
    {
        private string otodomUrl;

        private static Timer RunningTimer = null;
        private IConfigurationRoot configuration;
        private readonly IEmailService emailService;
        private readonly IFlatsFeedService flatsFeed;
        private readonly ILogger log;

        public string JobName => nameof(OtodomFeedJob);

        public OtodomFeedJob(
            IConfigurationRoot configuration,
            ILogger log,
            IFlatsFeedService flatsFeed)
        {
            this.configuration = configuration;
            this.log = log;
            this.flatsFeed = flatsFeed;
        }

        public void FeedOtodomData()
        {
            log.Info($"Processing {JobName}, time {DateTime.Now.ToLocalTime()}.");

            flatsFeed.StartFeedProcess(null);

            log.Info($"{JobName} updated: {flatsFeed.FeedStats.Updated}, added: {flatsFeed.FeedStats.Added}. Errors {flatsFeed.FeedStats.Errors.Count}");
            if (flatsFeed.FeedStats.Errors.Any())
            {
                log.Info($"Below errors happened during flats processing.");
                foreach (var error in flatsFeed.FeedStats.Errors)
                {
                    log.Info($"\t-> {error}");
                }
            }
            log.Info($"{JobName} finished, time {DateTime.Now.ToLocalTime()}.");
        }

        public void ImmediateRun()
        {
            log.Info($"Immediate execution of {JobName}.");
            FeedOtodomData();
            log.Info($"Job {JobName} done.");
        }

        public void Run()
        {
            FeedOtodomData();
        }
    }
}
