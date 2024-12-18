using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model.Inflation
{
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
}
