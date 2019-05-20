using MoreLinq;
using System.Collections.Generic;
using System.Linq;

namespace Core.Model.FlatsModels
{
    public class FlatAggregateVM
    {
        public IEnumerable<FlatDataBM> Flats { get; set; } = new List<FlatDataBM>();

        public Dictionary<string, List<FlatDataBM>> FlatsDictionary = new Dictionary<string, List<FlatDataBM>>();

        public List<FlatDataBM> SmallFlats => Flats.Where(x => x.FlatSize == FlatSizeEnum.Small && PriceAndMetersNotZero(x)).ToList();
        public List<FlatDataBM> MediumFlats => Flats.Where(x => x.FlatSize == FlatSizeEnum.Medium && PriceAndMetersNotZero(x)).ToList();
        public List<FlatDataBM> BigFlats => Flats.Where(x => x.FlatSize == FlatSizeEnum.Big && PriceAndMetersNotZero(x)).ToList();

        public List<FlatCalculationsVM> FlatCalculations = new List<FlatCalculationsVM>();

        public FlatAggregateVM(IEnumerable<FlatDataBM> flats)
        {
            Flats = flats;

            FlatsDictionary.Add(FlatSizeEnum.Small, SmallFlats);
            FlatsDictionary.Add(FlatSizeEnum.Medium, MediumFlats);
            FlatsDictionary.Add(FlatSizeEnum.Big, BigFlats);

            FlatCalculations.Add(Calculate(FlatSizeEnum.Small));
            FlatCalculations.Add(Calculate(FlatSizeEnum.Medium));
            FlatCalculations.Add(Calculate(FlatSizeEnum.Big));
        }

        private static bool PriceAndMetersNotZero(FlatDataBM x)
        {
            return x.SquareMeters != 0 && x.TotalPrice != 0;
        }

        private FlatCalculationsVM Calculate(string flatSize)
        {
            if (FlatsDictionary[flatSize] == null || FlatsDictionary[flatSize].Count == 0)
                return null;

            return new FlatCalculationsVM()
            {
                FlatSize = flatSize,
                Amount = FlatsDictionary[flatSize].Count,
                AvgPrice = FlatsDictionary[flatSize].Average(x => x.TotalPrice),
                BigestFlat = FlatsDictionary[flatSize].MaxBy(x => x.SquareMeters).FirstOrDefault(),
                SmallestFlat = FlatsDictionary[flatSize].MinBy(x => x.SquareMeters).FirstOrDefault(),
                MostExpensiveFlat = FlatsDictionary[flatSize].MaxBy(x => x.TotalPrice).FirstOrDefault(),
                CheapestFlat = FlatsDictionary[flatSize].MinBy(x => x.TotalPrice).FirstOrDefault(),
                AvgPricePerMeter = FlatsDictionary[flatSize].Average(x => x.PricePerMeter)
            };
        }
    }
}
