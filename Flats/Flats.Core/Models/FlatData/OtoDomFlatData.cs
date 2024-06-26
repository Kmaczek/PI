﻿using Core.Model.FlatsModels;

namespace Flats.Core.Models.FlatData
{
    public class OtoDomFlatData : FlatDataBm
    {
        public OtoDomFlatData(decimal squareMeters, decimal totalPrice, int rooms, string url, bool isPrivate)
            : base(squareMeters, totalPrice, rooms, url, isPrivate)
        {
        }

        public string City { get; set; }
        public string Street { get; set; }

        public bool IsPrivate { get; set; }
        public string Location { get; set; }
    }
}
