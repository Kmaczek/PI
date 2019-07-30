using Core.Common;
using Core.Domain.Logic.OtodomService;
using Core.Model.Exceptions;
using Data.EF.Models;
using Data.Repository.Interfaces;
using Flats.Core.Scraping;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Domain.Logic.FlatsFeed
{
    public class OtodomService : IFlatsFeedService
    {
        private readonly IConfigurationRoot configuration;
        private readonly IOtoDomRepository otoDomRepository;
        private readonly ILogger Log;
        private readonly Scraper otoDomScrapper;

        private string otodomUrl;
        private string urlSection = "";
        private string serviceName = nameof(OtodomService);

        public OtodomService(
            IConfigurationRoot configuration,
            ILogger log,
            Scraper otoDomScrapper,
            IOtoDomRepository otoDomRepository)
        {
            this.configuration = configuration;
            this.otoDomRepository = otoDomRepository;
            if (configuration == null)
                throw new OtodomServiceException($"Missing configuration provider for {serviceName}.");

            otodomUrl = configuration.GetSection("PI_XtbUserId").Value;
            if (string.IsNullOrEmpty(otodomUrl))
                throw new OtodomServiceException($"Missing configuration section [{urlSection}] for {serviceName}.");
        }

        private void StartFeedProcess()
        {
            otoDomScrapper.ScrapingUrl = otodomUrl;
            var scrappedData = otoDomScrapper.Scrape();

            //
            //otoDomRepository.SaveFlats(scrappedData.Select(x => new Flat()));
        }
    }
}
