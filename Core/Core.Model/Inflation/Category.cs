using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model.Inflation
{
    public class Category(int id, string name)
    {
        public int Id { get; set; } = id;
        public string Name { get; set; } = name;

        public static readonly Category FoodAndDrinks = new(1, "Żywność i napoje");
        public static readonly Category AlcoholAndTobaco = new(2, "Napoje alkoholowe i wyroby tytoniowe");
        public static readonly Category Clothing = new(3, "Odzież i obuwie");
        public static readonly Category HousingAndEnergy = new(4, "Dom i media");
        public static readonly Category EquipmentAndRunningHouse = new(5, "Wyposażenie i prowadzenie domu");
        public static readonly Category Healthcare = new(6, "Zdrowie");
        public static readonly Category Transport = new(7, "Transport");
        public static readonly Category Connectivity = new(8, "Łączność");
        public static readonly Category Recreation = new(9, "Rekreacja i kultura");
        public static readonly Category Education = new(10, "Edukacja");
        public static readonly Category RestaurantsAndHotels = new(11, "Restauracje i hotele");
        public static readonly Category Others = new(12, "Inne towary i usługi");

        public static Dictionary<int, Category> All = new()
        {
            { 1, FoodAndDrinks },
            { 2, AlcoholAndTobaco },
            { 3, Clothing },
            { 4, HousingAndEnergy },
            { 5, EquipmentAndRunningHouse },
            { 6, Healthcare },
            { 7, Transport },
            { 8, Connectivity },
            { 9, Recreation },
            { 10, Education },
            { 11, RestaurantsAndHotels },
            { 12, Others }
        };
    }
}
