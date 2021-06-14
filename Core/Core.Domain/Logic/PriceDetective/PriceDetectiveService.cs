using Core.Common;
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
    public class PriceDetectiveService: IPriceDetectiveService
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
        }

        public int Delay { 
            get
            {
                return Convert.ToInt32(this.configuration.GetSection(DelayConfigKey).Value, CultureInfo.InvariantCulture);
            }
        }

        public IEnumerable<PriceParserResult> CollectPriceData()
        {
            var priceData = new List<PriceParserResult>();
            var parserConfigs = priceRepository.GetParsers();
            foreach (var parserConfig in parserConfigs)
            {
                var parser = parsers[parserConfig.ParserType.Name]();
                parser.Load(new Uri(parserConfig.Uri));
                var result = parser.Parse();
                result.ParserConfigId = parserConfig.Id;

                priceData.Add(result);
                Thread.Sleep(Delay);
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
                var existingPriceDetail = dbPriceDetails
                    .Where(pd => pd.Title == price.Title && pd.RetailerNo == price.ProductNo)
                    .OrderByDescending(pd => pd.Id)
                    .FirstOrDefault();

                if (existingPriceDetail == null)
                {
                    var priceDetailsItem = new PriceDetails();
                    priceDetailsItem.Title = price.Title;
                    priceDetailsItem.RetailerNo = price.ProductNo;
                    priceDetailsItem.CreatedDate = dateNow;

                    priceRepository.SavePriceDetails(priceDetailsItem);
                    existingPriceDetail = priceDetailsItem;
                }

                var priceSerie = new PriceSeries();
                priceSerie.CreatedDate = dateNow;
                priceSerie.ParserId = price.ParserConfigId;
                priceSerie.Price = price.Price;
                priceSerie.PriceDetailsId = existingPriceDetail.Id;

                priceSeries.Add(priceSerie);
            }

            priceRepository.SavePrices(priceSeries);
        }
    }
}
