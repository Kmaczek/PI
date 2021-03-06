﻿using Data.EF.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repository.Interfaces
{
    public interface IBinanceRepository
    {
        void SaveSeries(Series cryptoSeries);
        void SaveSeriesParent(SeriesParent seriesParent);
    }
}
