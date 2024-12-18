using Inflation.Models;

namespace Inflation.Logic
{
    public class InflationCalculator
    {
        public InflationCalculator() { }

        public decimal CalculateInflation(Basket basket, List<Product> products)
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

            return inflation / weightSum;
        }
    }
}
