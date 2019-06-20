using System.Collections.Generic;

namespace Core.Model.FlatsModels
{
    public class FlatDataBM
    {
        public decimal SquareMeters { get; set; }
        public decimal TotalPrice { get; set; }
        public int Rooms { get; set; }
        public string Url { get; set; }

        public string FlatSize { get; set; }
        public decimal PricePerMeter { get; set; }

        public List<string> Errors { get; } = new List<string>();

        public FlatDataBM(
            decimal squareMeters,
            decimal totalPrice,
            int rooms,
            string url)
        {
            SquareMeters = squareMeters;
            TotalPrice = totalPrice;
            Rooms = rooms;
            Url = url;

            FlatSize = GetFlatSize(SquareMeters);
            if (SquareMeters != 0)
                PricePerMeter = TotalPrice / SquareMeters;
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
