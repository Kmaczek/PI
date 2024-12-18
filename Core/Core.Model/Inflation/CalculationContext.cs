using System.Collections.Generic;

namespace Core.Model.Inflation
{
    public class CalculationContext
    {
        public CalculationContext(int productId, IEnumerable<decimal> prices, IEnumerable<decimal> pricesYearAgo, Category category)
        {
            Id = productId;
            Prices = prices;
            PricesYearAgo = pricesYearAgo;
            Category = category;
        }

        public int Id { get; set; }
        public IEnumerable<decimal> Prices { get; set; }
        public IEnumerable<decimal> PricesYearAgo { get; set; }
        public Category Category { get; set; }
    }
}
