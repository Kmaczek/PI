﻿using Core.Model.FlatsModels;

namespace CenyMieszkan.Models.FlatData
{
    public class DomGratkaFlatData: FlatDataBM
    {
        public DomGratkaFlatData(decimal squareMeters, decimal totalPrice, string city, string street, int rooms, string url, string location, int year) 
            : base(squareMeters, totalPrice, rooms, url)
        {
            SquareMeters = squareMeters;
            TotalPrice = totalPrice;
            Url = url;
            Year = year;
        }

        public string City { get; set; }
        public string Street { get; set; }

        public string Location { get; set; }
        public int Year { get; set; }
    }
}