using Core.Model.PriceDetectiveModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Core.Domain.Logic.PriceDetective.PriceParsers
{
    public class FriscoPriceParser_v2 : IPriceParser
    {
        public void Load(Uri uri)
        {
            // Doesnt load
        }

        public IEnumerable<PriceParserResult> Parse()
        {
            var result = new PriceParserResult();

            return new List<PriceParserResult>() { result };
        }
    }
}
