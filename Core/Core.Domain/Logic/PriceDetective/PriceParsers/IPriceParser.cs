using Core.Model.PriceDetectiveModels;
using System;
using System.Collections.Generic;

namespace Core.Domain.Logic.PriceDetective.PriceParsers
{
    public interface IPriceParser
    {
        void Load(Uri uri);
        IEnumerable<PriceParserResult> Parse();
    }
}
