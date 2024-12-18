using Core.Common.Logging;
using Core.Domain.Logic.Inflation;
using Jobs.OldScheduler.Jobs.Parameters;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jobs.OldScheduler.Jobs
{
    public class InflationJob : IJob
    {
        private readonly IConfigurationRoot _configuration;
        private readonly ILogger _log;
        private readonly ICalculateInflation _inflationService;


        public InflationJob(
            IConfigurationRoot configuration,
            ILogger log,
            ICalculateInflation inflationService)
        {

            _configuration = configuration;
            _log = log;
            _inflationService = inflationService;
        }

        public string JobName => nameof(InflationJob);

        public void ImmediateRun()
        {
            _log.Info($"Immediate execution of {JobName}.");
            RunInflation();
            _log.Info($"Job {JobName} done.");
        }

        public void ImmediateRun(IEnumerable<string> stringParameters)
        {
            var convertedParameters = new InflationParameters(stringParameters);
            RunInflation(convertedParameters);
        }

        public void Run()
        {
            RunInflation();
        }

        private void RunInflation(InflationParameters parameters = null)
        {
            try
            {
                var x = _inflationService.CalculateInflation(parameters?.DatesToRun?.First() ?? DateTime.Now).Result;
            }
            catch (Exception e)
            {
                _log.Error($"Job {JobName} failed.", e);
            }
        }
    }
}
