namespace Core.Model.FlatsModels
{
    public class FlatCalculationsVm
    {
        private readonly FlatDataBm SmallestFlat;
        private readonly FlatDataBm BigestFlat;
        private readonly FlatDataBm CheapestFlat;
        private readonly FlatDataBm MostExpensiveFlat;

        public FlatCalculationsVm(
            FlatDataBm smallestFlat,
            FlatDataBm bigestFlat,
            FlatDataBm cheapestFlat,
            FlatDataBm mostExpensiveFlat
            )
        {
            SmallestFlat = smallestFlat;
            BigestFlat = bigestFlat;
            CheapestFlat = cheapestFlat;
            MostExpensiveFlat = mostExpensiveFlat;
        }

        public string FlatSize { get; set; }

        public int Amount { get; set; }

        [FormatAttribute(FormatType.Numeric2)]
        public decimal AvgPrice { get; set; }

        [FormatAttribute(FormatType.Numeric2)]
        public decimal AvgPricePerMeter { get; set; }


        [FormatAttribute(FormatType.Numeric0)]
        public decimal? Smallest => SmallestFlat?.Surface;
        public string SmallestLink => SmallestFlat?.Url;

        [FormatAttribute(FormatType.Numeric0)]
        public decimal? Bigest => BigestFlat?.Surface;
        public string BigestLink => BigestFlat?.Url;

        [FormatAttribute(FormatType.Numeric0)]
        public decimal? Cheapest => CheapestFlat?.TotalPrice;
        public string CheapestLink => CheapestFlat?.Url;

        [FormatAttribute(FormatType.Numeric0)]
        public decimal? MostExpensive => MostExpensiveFlat?.TotalPrice;
        public string MostExpensiveLink => MostExpensiveFlat?.Url;
    }
}
