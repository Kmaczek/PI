using Core.Model.PriceDetectiveModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Domain.Logic.PriceDetective
{
    public interface IPriceDetectiveService
    {
        void SavePrices(IEnumerable<PriceParserResult> priceData);
        IEnumerable<PriceParserResult> CollectPriceData();
    }
}
