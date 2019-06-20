using System.Collections.Generic;

namespace Core.Model.FlatsModels
{
    public class FlatOutput
    {
        public List<FlatCalculationsVM> PrivateFlatsByCategory { get; } = new List<FlatCalculationsVM>();

        public List<FlatCalculationsVM> AllFlatsByCategory { get; } = new List<FlatCalculationsVM>();

        public FlatOutput(List<FlatCalculationsVM> privateFlatsByCategory = null, List<FlatCalculationsVM> allFlatsByCategory = null)
        {
            if(privateFlatsByCategory != null)
                PrivateFlatsByCategory = privateFlatsByCategory;
            if(allFlatsByCategory != null)
                AllFlatsByCategory = allFlatsByCategory;
        }
    }
}
