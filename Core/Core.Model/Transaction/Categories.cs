using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Core.Model.Transaction
{
    public static class Categories
    {
        public const string Spozywcze = "Spożywcze";
        public const string Prezenty = "Prezenty";
        public const string Zdrowie = "Zdrowie";
        public const string Dom = "Dom";
        public const string Transport = "Transport";
        public const string Osobiste = "Osobiste";
        public const string Zwierzeta = "Zwierzeta";
        public const string Rachunki = "Rachunki";
        public const string Podroze = "Podroze";
        public const string Kredyt = "Kredyt";
        public const string Inne = "Inne";
        public const string OdziezObuwie = "Odziez/Obuwie";
        public const string ChemiaKosmetyki = "Chemia/Kosmetyki";
        public const string Rozrywka = "Rozrywka";
        public const string NieWiadomo = "Nie wiadomo";
        public const string Restauracje = "Restauracje";
        public const string Ubezpieczenie = "Ubezpieczenie";
        public const string Podatki = "Podatki";
        public const string Oszczednosci = "Oszczednosci";
        public const string Inwestycje = "Inwestycje";
        public const string Zabawki = "Zabawki";
        public const string StaleOplaty = "Stale Oplaty";

        private static readonly List<string> all = typeof(Categories)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(fi => fi.IsLiteral && !fi.IsInitOnly)
            .Select(fi => fi.GetValue(null).ToString())
            .ToList();

        public static List<string> All => all;
    }
}
