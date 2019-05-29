namespace Core.Model.FlatsModels
{
    public class FlatCalculationsVM
    {
        private readonly FlatDataBM SmallestFlat;
        private readonly FlatDataBM BigestFlat;
        private readonly FlatDataBM CheapestFlat;
        private readonly FlatDataBM MostExpensiveFlat;

        public FlatCalculationsVM(
            FlatDataBM smallestFlat,
            FlatDataBM bigestFlat,
            FlatDataBM cheapestFlat,
            FlatDataBM mostExpensiveFlat
            )
        {
            SmallestFlat = smallestFlat;
            BigestFlat = bigestFlat;
            CheapestFlat = cheapestFlat;
            MostExpensiveFlat = mostExpensiveFlat;
        }

        public string FlatSize { get; set; }

        public decimal Amount { get; set; }

        [FormatAttribute(FormatType.Numeric2)]
        public decimal AvgPrice { get; set; }

        [FormatAttribute(FormatType.Numeric2)]
        public decimal AvgPricePerMeter { get; set; }


        [FormatAttribute(FormatType.Numeric0)]
        public decimal? Smallest => SmallestFlat?.SquareMeters;
        public string SmallestLink => SmallestFlat?.Url;

        [FormatAttribute(FormatType.Numeric0)]
        public decimal? Bigest => BigestFlat?.SquareMeters;
        public string BigestLink => BigestFlat?.Url;

        [FormatAttribute(FormatType.Numeric0)]
        public decimal? Cheapest => CheapestFlat?.TotalPrice;
        public string CheapestLink => CheapestFlat?.Url;

        [FormatAttribute(FormatType.Numeric0)]
        public decimal? MostExpensive => MostExpensiveFlat?.TotalPrice;
        public string MostExpensiveLink => MostExpensiveFlat?.Url;
    }
}
