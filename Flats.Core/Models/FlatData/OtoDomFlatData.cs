using Core.Model.FlatsModels;

namespace CenyMieszkan.Models.FlatData
{
    public class OtoDomFlatData : FlatDataBM
    {
        public OtoDomFlatData(decimal squareMeters, decimal totalPrice, int rooms, string url) 
            : base(squareMeters, totalPrice, rooms, url)
        {
        }

        public string City { get; set; }
        public string Street { get; set; }

        public bool IsPrivate { get; set; }
        public string Location { get; set; }
    }
}
