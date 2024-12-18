using Core.Model.PriceView;
using Data.EF.Models;
using Data.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<bool> HealthCheck()
        {
            using var context = contextMaker.Invoke();
            var parsers = await context.Products.ToListAsync();

            return true;
        }

        public IEnumerable<Product> GetProductsActive()
        {
            var dateNow = DateTime.Now;
            using var context = contextMaker.Invoke();
            var parsers = context.Products
                .Include(p => p.ParserType)
                .Where(x => x.ActiveFrom < dateNow && dateNow < x.ActiveTo)
                .ToList();

            return parsers;
        }

        public async Task<Product> GetProduct(int productId)
        {
            using var context = contextMaker.Invoke();
            var product = await context.Products
                .Include(p => p.LatestPriceDetail)
                .SingleOrDefaultAsync(x => x.Id == productId);

            return product;
        }

        public async Task<IEnumerable<Product>> GetProducts(IEnumerable<int> productsToRun)
        {
            var dateNow = DateTime.Now;
            using var context = contextMaker.Invoke();
            var products = context.Products
                .Include(p => p.ParserType)
                .Where(x => x.ActiveFrom < dateNow && dateNow < x.ActiveTo);

            if (productsToRun != null)
            {
                products = products.Where(p => productsToRun.Contains(p.Id));
            }

            return await products.ToListAsync();
        }

        public IEnumerable<PriceDetails> GetPriceDetails()
        {
            using var context = contextMaker.Invoke();
            var parsers = context.PriceDetails.ToList();

            return parsers;
        }

        public async Task<IEnumerable<GrouppedProductsVm>> GetProductsGrouppedBySite()
        {
            using var context = contextMaker.Invoke();
            var parsers = await context.Products.Include(p => p.LatestPriceDetail).ToListAsync();
            var parserTypes = await context.ParserType.ToListAsync();
            var grouppedParsers = new List<GrouppedProductsVm>();

            foreach (var parserType in parserTypes)
            {
                var grouppedParser = new GrouppedProductsVm
                {
                    SiteId = parserType.Id,
                    SiteName = parserType.DisplayName,
                    Products = parsers
                        .Where(product => product.ParserTypeId == parserType.Id)
                        .Select(product => new ProductVm()
                        {
                            Id = product.Id,
                            LatestPriceDetailId = product.LatestPriceDetailId,
                            Code = product.LatestPriceDetail?.RetailerNo,
                            Name = product.LatestPriceDetail?.Title,
                            Uri = product.Uri,
                        })
                };

                grouppedParsers.Add(grouppedParser);
            }

            return grouppedParsers;
        }

        public IEnumerable<PriceSeries> GetPriceSeries(DateTime date)
        {
            using var context = contextMaker.Invoke();
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

        public async Task<IEnumerable<PriceSeries>> GetPriceSeries(int priceDetailsId)
        {
            using var context = contextMaker.Invoke();
            var priceSeries = await context.PriceSeries
                .Where(ps => ps.PriceDetailsId == priceDetailsId)
                .ToListAsync();

            return priceSeries;
        }

        public async Task<IEnumerable<PriceSeries>> GetPriceSeries(DateTime from, DateTime to)
        {
            using var context = contextMaker.Invoke();
            var priceSeries = await context.PriceSeries
                .Where(ps => ps.CreatedDate >= from && ps.CreatedDate < to)
                .ToListAsync();

            return priceSeries;
        }

        public IEnumerable<PriceSeries> GetMaxPricesDetails()
        {
            using var context = contextMaker.Invoke();
            var priceSeries = context.PriceSeries
                .Include(ps => ps.PriceDetails)
                .Include(ps => ps.Parser)
                .Where(p => p.Parser.Track)
                .ToList();

            var latestPrices = priceSeries.GroupBy(g => g.ParserId).Select(g => g.FirstOrDefault(ps => ps.Price == g.Max(y => y.Price))).ToList();

            return latestPrices;
        }

        public IEnumerable<PriceSeries> GetMinPricesDetails()
        {
            using var context = contextMaker.Invoke();
            var priceSeries = context.PriceSeries
                .Include(ps => ps.PriceDetails)
                .Include(ps => ps.Parser)
                .Where(p => p.Parser.Track)
                .ToList();

            var latestPrices = priceSeries.GroupBy(g => g.ParserId).Select(g => g.FirstOrDefault(ps => ps.Price == g.Min(y => y.Price))).ToList();

            return latestPrices;
        }

        public void SavePriceDetails(PriceDetails priceDetails, int ParserId)
        {
            using var context = contextMaker.Invoke();
            context.PriceDetails.Add(priceDetails);
            context.SaveChanges();

            context.Products.Find(ParserId).LatestPriceDetailId = priceDetails.Id;
            context.SaveChanges();
        }

        public void SavePrices(IEnumerable<PriceSeries> priceSeries)
        {
            using var context = contextMaker.Invoke();
            context.PriceSeries.AddRange(priceSeries);
            context.SaveChanges();
        }
    }
}
