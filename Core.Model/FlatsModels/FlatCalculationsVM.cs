namespace Core.Model.FlatsModels
{
    public class FlatCalculationsVM
    {
        public FlatDataBM SmallestFlat;
        public FlatDataBM BigestFlat;
        public FlatDataBM CheapestFlat;
        public FlatDataBM MostExpensiveFlat;

        public string FlatSize { get; set; }

        public decimal Amount { get; set; }

        [Format(FormatType.Numeric2)]
        public decimal AvgPrice { get; set; }

        [Format(FormatType.Numeric2)]
        public decimal AvgPricePerMeter { get; set; }


        [Format(FormatType.Numeric0)]
        public decimal? Smallest => SmallestFlat?.SquareMeters;
        public string SmallestLink => SmallestFlat?.Url;

        [Format(FormatType.Numeric0)]
        public decimal? Bigest => BigestFlat?.SquareMeters;
        public string BigestLink => BigestFlat?.Url;

        [Format(FormatType.Numeric0)]
        public decimal? Cheapest => CheapestFlat?.TotalPrice;
        public string CheapestLink => CheapestFlat?.Url;

        [Format(FormatType.Numeric0)]
        public decimal? MostExpensive => MostExpensiveFlat?.TotalPrice;
        public string MostExpensiveLink => MostExpensiveFlat?.Url;
    }
}
