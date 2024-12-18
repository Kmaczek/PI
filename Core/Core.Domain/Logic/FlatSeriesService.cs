using Core.Model.FlatSerie;
using Data.Repository.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Domain.Logic
{
    public class FlatSeriesService : IFlatSeriesService
    {
        private readonly ILogger<FlatSeriesService> logger;
        private readonly IOtoDomRepository otoDomRepository;

        public FlatSeriesService(
            ILogger<FlatSeriesService> logger,
            IOtoDomRepository otoDomRepository)
        {
            this.logger = logger;
            this.otoDomRepository = otoDomRepository;
        }

        public IEnumerable<FlatSerieVm> GetFlatSeries()
        {
            var flatSeries = otoDomRepository.GetFlatSeries();
            var flatSerieVmList = new List<FlatSerieVm>();
            foreach(var flatSerie in flatSeries)
            {
                var flatSeriesVm = new FlatSerieVm()
                {
                    Id = flatSerie.Id,
                    Amount = flatSerie.Amount,
                    AvgPrice = flatSerie.AvgPrice,
                    AvgPricePerMeter = flatSerie.AvgPricePerMeter,
                    Day = flatSerie.DateFetched.ToUniversalTime(),
                };

                flatSerieVmList.Add(flatSeriesVm);
            }

            return flatSerieVmList;
        }
    }
}
