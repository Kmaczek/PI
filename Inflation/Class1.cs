namespace Inflation;

public class Basket
{
    public string Name { get; set; }
    public List<ProductGroup> Groups { get; set; }

    const ProductGroup FoodAndDrinks = new ProductGroup (Category.FoodAndDrinks, 27.01);
    const ProductGroup AlcoholAndTobaco = new ProductGroup (Category.AlcoholAndTobaco, 5.75);
    const ProductGroup Clothing = new ProductGroup (Category.Clothing, 4.27);
    const ProductGroup HousingAndEnergy = new ProductGroup (Category.HousingAndEnergy, 19.63);
    const ProductGroup EquipmentAndRunningHouse = new ProductGroup (Category.EquipmentAndRunningHouse, 5.29);
    const ProductGroup Healthcare = new ProductGroup (Category.Healthcare, 5.71);
    const ProductGroup Transport = new ProductGroup (Category.Transport, 9.92);
    const ProductGroup Connectivity = new ProductGroup (Category.Connectivity, 4.48);
    const ProductGroup Recreation = new ProductGroup (Category.Recreation, 6.14);
    const ProductGroup Education = new ProductGroup (Category.Education, 2.21);
    const ProductGroup RestaurantsAndHotels = new ProductGroup (Category.RestaurantsAndHotels, 5.11);
    const ProductGroup Others = new ProductGroup (Category.Others, 5.48);
}

public class Product
{
    public Product(int id, string name, decimal price, decimal priceYearAgo, int groupId)
    {
        Id = id;
        Name = name;
        Price = price;
        PriceYearAgo = priceYearAgo;
        GroupId = groupId;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public decimal PriceYearAgo { get; set; }
    public int GroupId { get; set; }
}

public class ProductGroup
{
    public ProductGroup(int category, decimal weight)
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

    public const Category FoodAndDrinks = new Category (1, "Żywność i napoje");
    public const Category AlcoholAndTobaco = new Category (2, "Napoje alkoholowe i wyroby tytoniowe");
    public const Category Clothing = new Category (3, "Odzież i obuwie");
    public const Category HousingAndEnergy = new Category (4, "Dom i media");
    public const Category EquipmentAndRunningHouse = new Category (5, "Wyposażenie i prowadzenie domu");
    public const Category Healthcare = new Category (6, "Zdrowie");
    public const Category Transport = new Category (7, "Transport");
    public const Category Connectivity = new Category (8, "Łączność");
    public const Category Recreation = new Category (9, "Rekreacja i kultura");
    public const Category Education = new Category (10, "Edukacja");
    public const Category RestaurantsAndHotels = new Category (11, "Restauracje i hotele");
    public const Category Others = new Category (12, "Inne towary i usługi");
}