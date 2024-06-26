﻿using Core.Common.Logging;
using Core.Domain.Logic.PriceDetective.PriceParsers;
using Core.Model.PriceDetectiveModels;
using Data.EF.Models;
using Data.Repository.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;

namespace Core.Domain.Logic.PriceDetective
{
    public class PriceDetectiveService : IPriceDetectiveService
    {
        private const string DelayConfigKey = "PriceDetectiveJob:delay";

        private Dictionary<string, Func<IPriceParser>> parsers = new Dictionary<string, Func<IPriceParser>>();

        private readonly IConfigurationRoot configuration;
        private readonly ILogger log;
        private readonly IPriceRepository priceRepository;

        public PriceDetectiveService(
            IConfigurationRoot configuration,
            ILogger log,
            IPriceRepository priceRepository)
        {
            this.configuration = configuration;
            this.log = log;
            this.priceRepository = priceRepository;

            parsers.Add(nameof(XcomPriceParser), () => new XcomPriceParser());
            parsers.Add(nameof(FriscoPriceParser), () => new FriscoPriceParser());
            parsers.Add(nameof(CastoramaPriceParser), () => new CastoramaPriceParser());
            parsers.Add(nameof(DozPriceParser), () => new DozPriceParser());
        }

        public int Delay
        {
            get
            {
                return Convert.ToInt32(this.configuration.GetSection(DelayConfigKey).Value, CultureInfo.InvariantCulture);
            }
        }

        public IEnumerable<PriceParserResult> CollectPriceData(IEnumerable<int> parsersToRun = null)
        {
            var priceData = new List<PriceParserResult>();
            var parserConfigs = parsersToRun != null ? priceRepository.GetParsers(parsersToRun) : priceRepository.GetParsers();

            foreach (var parserConfig in parserConfigs)
            {
                var parser = parsers[parserConfig.ParserType.Name]();
                try
                {
                    parser.Load(new Uri(parserConfig.Uri));
                    var results = parser.Parse();
                    foreach(var result in results)
                    {
                        result.ParserConfigId = parserConfig.Id;
                        if (result.Proper)
                            priceData.Add(result);
                    }
                    
                    Thread.Sleep(Delay);
                }
                catch (Exception e)
                {
                    log.Error($"Error while parsing {parserConfig.Id} - {parserConfig.Uri}", e);
                }

            }

            return priceData;
        }

        public void SavePrices(IEnumerable<PriceParserResult> priceData)
        {
            var dateNow = DateTime.Now;

            var priceSeries = new List<PriceSeries>();
            var dbPriceDetails = priceRepository.GetPriceDetails();
            foreach (var price in priceData)
            {
                var priceSerie = new PriceSeries();
                priceSerie.CreatedDate = dateNow;
                priceSerie.ParserId = price.ParserConfigId;
                priceSerie.Price = price.Price;

                var existingPriceDetail = dbPriceDetails
                    .Where(pd => pd.RetailerNo == price.ProductNo)
                    .OrderByDescending(pd => pd.Id)
                    .FirstOrDefault();

                if (existingPriceDetail == null)
                {
                    var priceDetailsItem = new PriceDetails();
                    priceDetailsItem.Title = price.Title;
                    priceDetailsItem.RetailerNo = price.ProductNo;
                    priceDetailsItem.CreatedDate = dateNow;

                    priceRepository.SavePriceDetails(priceDetailsItem, priceSerie.ParserId);
                    existingPriceDetail = priceDetailsItem;
                }

                priceSerie.PriceDetailsId = existingPriceDetail.Id;

                priceSeries.Add(priceSerie);
            }

            priceRepository.SavePrices(priceSeries);
        }
    }
}
