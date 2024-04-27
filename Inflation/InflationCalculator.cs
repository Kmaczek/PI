namespace Inflation;

public class InflationCalculator
{
    public decimal CalculateInflation(Basket basket, List<Product> products)
    {
        decimal inflation = 0;
        foreach (var group in basket.Groups)
        {
            var productsInGroup = products.Where(p => p.GroupId == group.Category.Id);
            decimal groupPrice = productsInGroup.Sum(p => p.Price);
            inflation += groupPrice * group.Weight;
        }
        return inflation;
    }
}