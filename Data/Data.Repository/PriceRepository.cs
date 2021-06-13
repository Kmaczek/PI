using Data.EF.Models;
using Data.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Data.Repository
{
    public class PriceRepository : IPriceRepository
    {
        PiContext PiContext;
        private readonly Func<PiContext> contextMaker;

        public PriceRepository(Func<PiContext> contextMaker)
        {
            this.contextMaker = contextMaker;
            PiContext = contextMaker.Invoke();
        }

        public void Dispose()
        {
            PiContext.Dispose();
        }

        public IEnumerable<Parser> GetParsers()
        {
            var dateNow = DateTime.Now;
            using (var context = contextMaker.Invoke())
            {
                var parsers = context.Parser
                    .Include(p => p.ParserType)
                    .Where(x => x.ActiveFrom < dateNow && dateNow < x.ActiveTo )
                    .ToList();

                return parsers;
            }
        }
        
        public IEnumerable<PriceDetails> GetPriceDetails()
        {
            var dateNow = DateTime.Now;
            using (var context = contextMaker.Invoke())
            {
                var parsers = context.PriceDetails.ToList();

                return parsers;
            }
        }

        public IEnumerable<PriceSeries> GetTodaysPricesDetails()
        {
            var dateNow = DateTime.Now;
            using (var context = contextMaker.Invoke())
            {
                var priceSeries = context.PriceSeries
                    .Include(ps => ps.PriceDetails)
                    .Include(ps => ps.Parser)
                    .Where(p => dateNow > p.CreatedDate && p.CreatedDate > dateNow.AddDays(-1))
                    .ToList();

                var latestPrices = priceSeries.GroupBy(g => g.ParserId).Select(g => g.FirstOrDefault(ps => ps.Id == g.Max(y => y.Id))).ToList();

                return latestPrices;
            }
        }

        public void SavePriceDetails(PriceDetails priceDetails)
        {
            var dateNow = DateTime.Now;

            using (var context = contextMaker.Invoke())
            {
                context.PriceDetails.Add(priceDetails);
                context.SaveChanges();
            }
        }

        public void SavePrices(IEnumerable<PriceSeries> priceSeries)
        {
            var dateNow = DateTime.Now;

            using (var context = contextMaker.Invoke())
            {
                context.PriceSeries.AddRange(priceSeries);
                context.SaveChanges();
            }
        }
    }
}
