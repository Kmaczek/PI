namespace Inflation.Models;

public class Basket
{
    public string Name { get; set; }
    public List<ProductGroup> Groups { get; set; } = [];

    readonly ProductGroup FoodAndDrinks = new(Category.FoodAndDrinks, 27.01m);
    readonly ProductGroup AlcoholAndTobaco = new(Category.AlcoholAndTobaco, 5.75m);
    readonly ProductGroup Clothing = new(Category.Clothing, 4.27m);
    readonly ProductGroup HousingAndEnergy = new(Category.HousingAndEnergy, 19.63m);
    readonly ProductGroup EquipmentAndRunningHouse = new(Category.EquipmentAndRunningHouse, 5.29m);
    readonly ProductGroup Healthcare = new(Category.Healthcare, 5.71m);
    readonly ProductGroup Transport = new(Category.Transport, 9.92m);
    readonly ProductGroup Connectivity = new(Category.Connectivity, 4.48m);
    readonly ProductGroup Recreation = new(Category.Recreation, 6.14m);
    readonly ProductGroup Education = new(Category.Education, 2.21m);
    readonly ProductGroup RestaurantsAndHotels = new(Category.RestaurantsAndHotels, 5.11m);
    readonly ProductGroup Others = new(Category.Others, 5.48m);
}

public class Product
{
    public Product(int id, string name, IEnumerable<decimal> prices, IEnumerable<decimal> pricesYearAgo, Category category)
    {
        Id = id;
        Name = name;
        Prices = prices;
        PricesYearAgo = pricesYearAgo;
        Category = category;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public IEnumerable<decimal> Prices { get; set; }
    public IEnumerable<decimal> PricesYearAgo { get; set; }
    public Category Category { get; set; }
}

public class ProductGroup
{
    public ProductGroup(Category category, decimal weight)
    {
        Category = category;
        Weight = weight;
    }

    public Category Category { get; set; }
    public decimal Weight { get; set; }
}

public class Category
{
    public Category(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public int Id { get; set; }
    public string Name { get; set; }

    public static readonly Category FoodAndDrinks = new (1, "Żywność i napoje");
    public static readonly Category AlcoholAndTobaco = new (2, "Napoje alkoholowe i wyroby tytoniowe");
    public static readonly Category Clothing = new (3, "Odzież i obuwie");
    public static readonly Category HousingAndEnergy = new (4, "Dom i media");
    public static readonly Category EquipmentAndRunningHouse = new (5, "Wyposażenie i prowadzenie domu");
    public static readonly Category Healthcare = new (6, "Zdrowie");
    public static readonly Category Transport = new (7, "Transport");
    public static readonly Category Connectivity = new (8, "Łączność");
    public static readonly Category Recreation = new (9, "Rekreacja i kultura");
    public static readonly Category Education = new (10, "Edukacja");
    public static readonly Category RestaurantsAndHotels = new (11, "Restauracje i hotele");
    public static readonly Category Others = new (12, "Inne towary i usługi");
}