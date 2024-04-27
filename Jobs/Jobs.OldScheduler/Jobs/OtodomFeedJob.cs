using Core.Common.Logging;
using Core.Domain.Logic.FlatsFeed;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Jobs.OldScheduler.Jobs
{
    public class OtodomFeedJob : IJob
    {
        private static Timer RunningTimer = null;
        private IConfigurationRoot _configuration;
        private readonly IFlatsFeedService _flatsFeed;
        private readonly ILogger _log;

        public string JobName => nameof(OtodomFeedJob);

        public OtodomFeedJob(
            IConfigurationRoot configuration,
            ILogger log,
            IFlatsFeedService flatsFeed)
        {
            _configuration = configuration;
            _log = log;
            _flatsFeed = flatsFeed;
        }

        public void FeedOtodomData()
        {
            _log.Info($"Processing {JobName}, time {DateTime.Now.ToLocalTime()}.");

            try
            {
                //@"https://www.otodom.pl/sprzedaz/nowe-mieszkanie/?search%5Bfilter_float_price%3Ato%5D=500000&search%5Bfilter_float_price_per_m%3Ato%5D=6000&search%5Bfilter_float_m%3Afrom%5D=60&search%5Bfilter_enum_rooms_num%5D%5B0%5D=3&search%5Bdescription%5D=1&locations%5B0%5D%5Bcity_id%5D=39&locations%5B0%5D%5Bdistrict_id%5D=3&locations%5B0%5D%5Bstreet_id%5D=0&locations%5B1%5D%5Bregion_id%5D=1&locations%5B1%5D%5Bsubregion_id%5D=381&locations%5B1%5D%5Bcity_id%5D=39"
                _flatsFeed.StartFeedProcess(null);

                _log.Info($"{JobName} updated: {_flatsFeed.FeedStats.Updated}, added: {_flatsFeed.FeedStats.Added}. Errors {_flatsFeed.FeedStats.Errors.Count}");
                if (_flatsFeed.FeedStats.Errors.Any())
                {
                    _log.Info($"Below errors happened during flats processing.");
                    foreach (var error in _flatsFeed.FeedStats.Errors)
                    {
                        _log.Info($"\t-> {error}");
                    }
                }
                _log.Info($"{JobName} finished, time {DateTime.Now.ToLocalTime()}.");
            }
            catch(Exception e)
            {
                _log.Error($"Exception happened when trying to process otodom data.", e);
            }
        }

        public void ImmediateRun()
        {
            _log.Info($"Immediate execution of {JobName}.");
            FeedOtodomData();
            _log.Info($"Job {JobName} done.");
        }

        public void ImmediateRun(IEnumerable<string> parameters)
        {
            throw new NotImplementedException();
        }

        public void Run()
        {
            FeedOtodomData();
        }
    }
}
