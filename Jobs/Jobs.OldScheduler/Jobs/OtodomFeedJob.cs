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

            //@"https://www.otodom.pl/sprzedaz/nowe-mieszkanie/?search%5Bfilter_float_price%3Ato%5D=500000&search%5Bfilter_float_price_per_m%3Ato%5D=6000&search%5Bfilter_float_m%3Afrom%5D=60&search%5Bfilter_enum_rooms_num%5D%5B0%5D=3&search%5Bdescription%5D=1&locations%5B0%5D%5Bcity_id%5D=39&locations%5B0%5D%5Bdistrict_id%5D=3&locations%5B0%5D%5Bstreet_id%5D=0&locations%5B1%5D%5Bregion_id%5D=1&locations%5B1%5D%5Bsubregion_id%5D=381&locations%5B1%5D%5Bcity_id%5D=39"
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
