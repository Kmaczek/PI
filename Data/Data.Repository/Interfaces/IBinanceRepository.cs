using Data.EF.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repository.Interfaces
{
    public interface IBinanceRepository
    {
        void SaveSeries(BinanceSeries cryptoSeries);
        void SaveSeriesParent(SeriesParent seriesParent);
    }
}
