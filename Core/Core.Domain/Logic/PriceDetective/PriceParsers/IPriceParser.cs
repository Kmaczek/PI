using Core.Model.PriceDetectiveModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Domain.Logic.PriceDetective.PriceParsers
{
    public interface IPriceParser
    {
        void Load(Uri uri);
        PriceParserResult Parse();
    }
}
