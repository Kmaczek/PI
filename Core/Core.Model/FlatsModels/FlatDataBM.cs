using System.Collections.Generic;

namespace Core.Model.FlatsModels
{
    public class FlatDataBm
    {
        public string OtoDomId { get; set; }
        public decimal Surface { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal? Rent { get; set; }
        public int Rooms { get; set; }
        public bool IsPrivate { get; set; }
        public int? Floor { get; set; }
        public int FloorNo { get; set; }
        public int? ConstructionYear { get; set; }
        public string Market { get; set; }
        public string Heating { get; set; }
        public string Location { get; set; }
        public string TypeOfProperty { get; set; }
        public string FlatType { get; set; }
        public string Url { get; set; }

        public string FlatSize { get; set; }
        public decimal PricePerMeter { get; set; }

        public List<string> Errors { get; } = new List<string>();

        public FlatDataBm()
        {

        }

        public FlatDataBm(
            decimal squareMeters,
            decimal totalPrice,
            int rooms,
            string url,
            bool isPrivate)
        {
            Surface = squareMeters;
            TotalPrice = totalPrice;
            Rooms = rooms;
            Url = url;
            IsPrivate = isPrivate;

            FlatSize = GetFlatSize(Surface);
            if (Surface != 0)
                PricePerMeter = TotalPrice / Surface;
            else
                PricePerMeter = 0;
        }

        private string GetFlatSize(decimal squareMeters)
        {
            if (squareMeters < 45)
                return FlatSizeEnum.Small;
            else if (squareMeters >= 45 && squareMeters < 75)
                return FlatSizeEnum.Medium;
            else if (squareMeters >= 75)
                return FlatSizeEnum.Big;

            return FlatSizeEnum.Small;
        }
    }
}
