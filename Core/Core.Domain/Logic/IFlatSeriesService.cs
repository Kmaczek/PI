using Core.Model.FlatSerie;
using System.Collections.Generic;

namespace Core.Domain.Logic
{
    public interface IFlatSeriesService
    {
        IEnumerable<FlatSerieVm> GetFlatSeries();
    }
}
