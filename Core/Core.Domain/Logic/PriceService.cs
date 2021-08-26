using Core.Model.PriceView;
using Data.Repository.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Domain.Logic
{
    public class PriceService: IPriceService
    {
        private readonly ILogger<FlatSeriesService> _logger;
        private readonly IPriceRepository _priceRepository;

        public PriceService(
            ILogger<FlatSeriesService> logger,
            IPriceRepository priceRepository)
        {
            _logger = logger;
            _priceRepository = priceRepository;
        }

        public IEnumerable<GrouppedProductsVm> GetProducts()
        {
            var priceDetails = _priceRepository.GetProductsGrouppedBySite();

            return priceDetails;
        }

        public IEnumerable<PriceSeriesVm> GetPriceSeries(int productId)
        {
            var priceSeries = _priceRepository.GetPriceSeries(productId);
            var series = new List<PriceSeriesVm>();
            foreach (var ps in priceSeries)
            {
                var serie = new PriceSeriesVm();
                serie.Price = ps.Price;
                serie.CreatedDate = ps.CreatedDate;

                series.Add(serie);
            }

            return series;
        }
    }
}
