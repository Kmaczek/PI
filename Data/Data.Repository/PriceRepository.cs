using Core.Model.PriceView;
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
                    .Where(x => x.ActiveFrom < dateNow && dateNow < x.ActiveTo)
                    .ToList();

                return parsers;
            }
        }

        public IEnumerable<Parser> GetParsers(IEnumerable<int> parsersToRun)
        {
            var dateNow = DateTime.Now;
            using (var context = contextMaker.Invoke())
            {
                var parsers = context.Parser
                    .Include(p => p.ParserType)
                    .Where(x => x.ActiveFrom < dateNow && dateNow < x.ActiveTo);

                if (parsersToRun != null)
                {
                    parsers = parsers.Where(p => parsersToRun.Contains(p.Id));
                }

                return parsers.ToList();
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

        public IEnumerable<GrouppedProductsVm> GetProductsGrouppedBySite()
        {
            var dateNow = DateTime.Now;
            using (var context = contextMaker.Invoke())
            {
                var parsers = context.Parser.Include(p => p.LatestPriceDetail).ToList();
                var parserTypes = context.ParserType.ToList();
                var grouppedParsers = new List<GrouppedProductsVm>();

                foreach (var parserType in parserTypes)
                {
                    var grouppedParser = new GrouppedProductsVm
                    {
                        SiteId = parserType.Id,
                        SiteName = parserType.DisplayName,
                        Products = parsers
                            .Where(p => p.ParserTypeId == parserType.Id)
                            .Select(ps => new ProductVm()
                            {
                                Id = ps.LatestPriceDetailId,
                                Code = ps.LatestPriceDetail?.RetailerNo ?? "Error",
                                Name = ps.LatestPriceDetail?.Title ?? "Error",
                                Uri = ps.Uri
                            })
                    };

                    grouppedParsers.Add(grouppedParser);
                }

                return grouppedParsers;
            }
        }

        public IEnumerable<PriceSeries> GetPriceSeries(DateTime date)
        {
            var dateNow = DateTime.Now;
            using (var context = contextMaker.Invoke())
            {
                var priceSeries = context.PriceSeries
                    .Include(ps => ps.PriceDetails)
                    .Include(ps => ps.Parser)
                    .Where(p =>
                        p.Parser.Track &&
                        date.AddDays(-1) < p.CreatedDate && p.CreatedDate < date)
                    .ToList();

                var latestPrices = priceSeries.GroupBy(g => g.ParserId).Select(g => g.FirstOrDefault(ps => ps.Id == g.Max(y => y.Id))).ToList();

                return latestPrices;
            }
        }

        public IEnumerable<PriceSeries> GetPriceSeries(int productId)
        {
            using (var context = contextMaker.Invoke())
            {
                var priceSeries = context.PriceSeries
                    .Where(ps => ps.PriceDetailsId == productId)
                    .ToList();

                return priceSeries;
            }
        }

        public IEnumerable<PriceSeries> GetMaxPricesDetails()
        {
            using (var context = contextMaker.Invoke())
            {
                var priceSeries = context.PriceSeries
                    .Include(ps => ps.PriceDetails)
                    .Include(ps => ps.Parser)
                    .Where(p => p.Parser.Track)
                    .ToList();

                var latestPrices = priceSeries.GroupBy(g => g.ParserId).Select(g => g.FirstOrDefault(ps => ps.Price == g.Max(y => y.Price))).ToList();

                return latestPrices;
            }
        }

        public IEnumerable<PriceSeries> GetMinPricesDetails()
        {
            using (var context = contextMaker.Invoke())
            {
                var priceSeries = context.PriceSeries
                    .Include(ps => ps.PriceDetails)
                    .Include(ps => ps.Parser)
                    .Where(p => p.Parser.Track)
                    .ToList();

                var latestPrices = priceSeries.GroupBy(g => g.ParserId).Select(g => g.FirstOrDefault(ps => ps.Price == g.Min(y => y.Price))).ToList();

                return latestPrices;
            }
        }

        public void SavePriceDetails(PriceDetails priceDetails, int ParserId)
        {
            var dateNow = DateTime.Now;

            using (var context = contextMaker.Invoke())
            {
                context.PriceDetails.Add(priceDetails);
                context.SaveChanges();

                context.Parser.Find(ParserId).LatestPriceDetailId = priceDetails.Id;
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
