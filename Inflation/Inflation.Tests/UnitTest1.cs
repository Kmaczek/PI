using Inflation.Logic;
using Inflation.Models;

namespace Inflation.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var calculator = new InflationCalculator();
            var basket = Baskets.GusBasket2023;
            var products = new List<Product>
            {
                new(1, "Product1", 10, 9, Category.FoodAndDrinks),
                new(2, "Product2", 29, 27, Category.FoodAndDrinks), 
                new(3, "Product3", 132, 130, Category.HousingAndEnergy)
            };

            calculator.CalculateInflation(basket, products);

            Console.WriteLine("Inflation: " + calculator.CalculateInflation(basket, products));
            Assert.Pass();
        }
    }
}