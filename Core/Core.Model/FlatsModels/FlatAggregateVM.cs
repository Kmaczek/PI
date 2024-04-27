using System.Collections.Generic;
using System.Linq;

namespace Core.Model.FlatsModels
{
    public class FlatAggregateVm
    {
        public IEnumerable<FlatDataBm> Flats { get; set; } = new List<FlatDataBm>();

        public List<FlatDataBm> SmallFlats => Flats.Where(x => x.FlatSize == FlatSizeEnum.Small && PriceAndMetersNotZero(x)).ToList();
        public List<FlatDataBm> MediumFlats => Flats.Where(x => x.FlatSize == FlatSizeEnum.Medium && PriceAndMetersNotZero(x)).ToList();
        public List<FlatDataBm> BigFlats => Flats.Where(x => x.FlatSize == FlatSizeEnum.Big && PriceAndMetersNotZero(x)).ToList();

        public Dictionary<string, List<FlatDataBm>> FlatsDictionary { get; } = new Dictionary<string, List<FlatDataBm>>();
        public List<FlatCalculationsVm> FlatCalculations { get; } = new List<FlatCalculationsVm>();

        public FlatAggregateVm(IEnumerable<FlatDataBm> flats)
        {
            Flats = flats;

            FlatsDictionary.Add(FlatSizeEnum.Small, SmallFlats);
            FlatsDictionary.Add(FlatSizeEnum.Medium, MediumFlats);
            FlatsDictionary.Add(FlatSizeEnum.Big, BigFlats);

            FlatCalculations.Add(Calculate(FlatSizeEnum.Small));
            FlatCalculations.Add(Calculate(FlatSizeEnum.Medium));
            FlatCalculations.Add(Calculate(FlatSizeEnum.Big));
        }

        private static bool PriceAndMetersNotZero(FlatDataBm x)
        {
            return x.Surface != 0 && x.TotalPrice != 0;
        }

        private FlatCalculationsVm Calculate(string flatSize)
        {
            if (FlatsDictionary[flatSize] == null || FlatsDictionary[flatSize].Count == 0)
                return null;

            return new FlatCalculationsVm(
                FlatsDictionary[flatSize].MinBy(x => x.Surface),
                FlatsDictionary[flatSize].MaxBy(x => x.Surface),
                FlatsDictionary[flatSize].MinBy(x => x.TotalPrice),
                FlatsDictionary[flatSize].MaxBy(x => x.TotalPrice))
            {
                FlatSize = flatSize,
                Amount = FlatsDictionary[flatSize].Count,
                AvgPrice = FlatsDictionary[flatSize].Average(x => x.TotalPrice),
                AvgPricePerMeter = FlatsDictionary[flatSize].Average(x => x.PricePerMeter)
            };
        }
    }
}
