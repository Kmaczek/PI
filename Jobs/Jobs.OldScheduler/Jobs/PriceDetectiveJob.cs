using Core.Common;
using Core.Domain.Logic.PriceDetective;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace Jobs.OldScheduler.Jobs
{
    public class PriceDetectiveJob : IJob
    {
        private readonly IConfigurationRoot _configuration;
        private readonly ILogger _log;
        private readonly IPriceDetectiveService priceDetectiveService;


        public PriceDetectiveJob(
            IConfigurationRoot configuration,
            ILogger log,
            IPriceDetectiveService priceDetectiveService)
        {

            this._configuration = configuration;
            this._log = log;
            this.priceDetectiveService = priceDetectiveService;
        }

        public string JobName => nameof(PriceDetectiveJob);

        public void ImmediateRun()
        {
            _log.Info($"Immediate execution of {JobName}.");
            RunPriceDetective();
            _log.Info($"Job {JobName} done.");
        }

        public void ImmediateRun(IEnumerable<string> stringParameters)
        {
            var convertedParameters = new PriceDetectiveParameters(stringParameters);
            RunPriceDetective(convertedParameters);
        }

        public void Run()
        {
            RunPriceDetective();
        }

        private void RunPriceDetective(PriceDetectiveParameters parameters = null)
        {
            try
            {
                var priceData = priceDetectiveService.CollectPriceData(parameters?.ParsersToRun);
                priceDetectiveService.SavePrices(priceData);
            }
            catch (Exception e)
            {
                _log.Error($"Job {JobName} failed.", e);
            }
        }
    }
}
