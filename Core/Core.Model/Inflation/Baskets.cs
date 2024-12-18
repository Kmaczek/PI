using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model.Inflation
{
    public class Baskets
    {
        public static Basket GusBasket2023 { get; set; } = new Basket()
        {
            Groups =
            [
                new(Category.FoodAndDrinks, 27.01m),
                new(Category.AlcoholAndTobaco, 5.75m),
                new(Category.Clothing, 4.27m),
                new(Category.HousingAndEnergy, 19.63m),
                new(Category.EquipmentAndRunningHouse, 5.29m),
                new(Category.Healthcare, 5.71m),
                new(Category.Transport, 9.92m),
                new(Category.Connectivity, 4.48m),
                new(Category.Recreation, 6.14m),
                new(Category.Education, 2.21m),
                new(Category.RestaurantsAndHotels, 5.11m),
                new(Category.Others, 5.48m),
            ]
        };
    }
}
