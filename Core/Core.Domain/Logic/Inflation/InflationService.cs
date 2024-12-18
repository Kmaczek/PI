using Core.Model.Inflation;
using Data.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Domain.Logic.Inflation
{
    public class InflationService(
        IPriceRepository _priceRepository) : ICalculateInflation
    {
        public async Task<decimal> CalculateInflation(DateTime date)
        {
            // starting prices
            var from = new DateTime(date.Year, date.Month, 1);
            var to = from.AddMonths(1);
            var pricesFromSelectedMonth = await _priceRepository.GetPriceSeries(from, to);
            var productIds = pricesFromSelectedMonth.Select(x => x.ParserId).Distinct();
            var storedProducts = await _priceRepository.GetProducts(productIds);

            // year back prices
            var fromYearAgo = new DateTime(date.Year - 1, date.Month, 1);
            var toYearAgo = fromYearAgo.AddMonths(1);
            var pricesFromYearBackMonth = await _priceRepository.GetPriceSeries(fromYearAgo, toYearAgo);

            var products = new List<CalculationContext>();
            foreach (var selectedMonthPricesGroup in pricesFromSelectedMonth.GroupBy(p => p.ParserId))
            {
                var yearAgoSeries = pricesFromYearBackMonth.Where(x => x.ParserId == selectedMonthPricesGroup.Key);
                if (!yearAgoSeries.Any())
                {
                    continue;
                }
                // Map series to category
                var category = Category.All[storedProducts.First(x => x.Id == selectedMonthPricesGroup.Key).Category];
                products.Add(new CalculationContext(selectedMonthPricesGroup.Key, selectedMonthPricesGroup.Select(x => x.Price), yearAgoSeries.Select(x => x.Price), category));
            }

            var inflation = CalculateInflation(Baskets.GusBasket2023, products);

            return inflation;
        }

        public decimal CalculateInflation(Basket basket, List<CalculationContext> products)
        {
            decimal inflation = 0;
            decimal weightSum = 0;
            foreach (var group in basket.Groups)
            {
                var currentPrice = products.Where(p => p.Category == group.Category).Sum(x => x.Prices.Average());
                var yearAgoPrice = products.Where(p => p.Category == group.Category).Sum(x => x.PricesYearAgo.Average());
                if (currentPrice == 0)
                {
                    continue;
                }

                inflation += (currentPrice - yearAgoPrice) / yearAgoPrice * 100 * group.Weight;
                weightSum += group.Weight;
            }

            if (weightSum == 0) return 0;

            return inflation / weightSum;
        }
    }
}
