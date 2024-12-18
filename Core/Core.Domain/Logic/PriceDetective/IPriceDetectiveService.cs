using Core.Model.PriceDetectiveModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Logic.PriceDetective
{
    public interface IPriceDetectiveService
    {
        void SavePrices(IEnumerable<PriceParserResult> priceData);
        Task<IEnumerable<PriceParserResult>> CollectPriceData(IEnumerable<int> parsersToRun = null);
    }
}
