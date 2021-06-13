using Core.Domain.Logic.EmailGeneration;
using System;
using System.Collections.Generic;

namespace Core.Domain.Logic
{
    public class EmailGeneratorFactory : IEmailGeneratorFactory
    {
        Dictionary<EmailGenerator, Func<IHtmlGenerator>> generators = new Dictionary<EmailGenerator, Func<IHtmlGenerator>>();

        public EmailGeneratorFactory(
            Func<XtbHtmlGenerator> xtbGenerator,
            Func<BinanceHtmlGenerator> binanceGenerator,
            Func<OtodomHtmlGenerator> otodomGenerator,
            Func<PriceDetectiveGenerator> priceDetectiveGenerator)
        {
            generators.Add(EmailGenerator.Xtb, xtbGenerator);
            generators.Add(EmailGenerator.Binance, binanceGenerator);
            generators.Add(EmailGenerator.Otodom, otodomGenerator);
            generators.Add(EmailGenerator.PriceDetective, priceDetectiveGenerator);
        }

        public IHtmlGenerator GetGenerator(EmailGenerator generatorType)
        {
            return generators[generatorType]();
        }
    }

    public enum EmailGenerator
    {
        Xtb = 1,
        Binance,
        Otodom,
        PriceDetective
    }
}
