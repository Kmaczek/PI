using Core.Domain.Logic.PriceDetective.PriceParsers;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.PriceParser.Parsers
{
    public class CastoramaPriceParserTests
    {
        [Test]
        public void ShouldParse()
        {
            var castoramaParser = new CastoramaPriceParser();
            castoramaParser.Load(new Uri("https://www.castorama.pl/deska-szalunkowa-blooma-impregnowana-22-x-100-x-3000-mm-id-1094996.html"));

            var result = castoramaParser.Parse();

            Assert.IsTrue(result.First().Proper);
            Assert.NotZero(result.First().Price);
            Assert.IsNotEmpty(result.First().ProductNo);
            Assert.IsNotEmpty(result.First().Title);
        }

    }
}
