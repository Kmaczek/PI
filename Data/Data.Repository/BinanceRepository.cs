using Data.EF.Models;
using Data.Repository.Interfaces;
using System;

namespace Data.Repository
{
    public class BinanceRepository : IBinanceRepository, IDisposable
    {
        PiContext PiContext;

        public BinanceRepository(PiContext dbContext)
        {
            PiContext = dbContext;
        }

        public void Dispose()
        {
            PiContext.Dispose();
        }

        public void SaveSeries(BinanceSeries cryptoSeries)
        {
            PiContext.BinanceSeries.Add(cryptoSeries);
            PiContext.SaveChanges();
        }

        public void SaveSeriesParent(SeriesParent seriesParent)
        {
            PiContext.SeriesParent.Add(seriesParent);
            PiContext.SaveChanges();
        }
    }
}
