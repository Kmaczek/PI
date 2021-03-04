using System.Collections.Generic;

namespace Core.Model.FlatsModels
{
    public class FlatOutput
    {
        public List<FlatCalculationsVm> PrivateFlatsByCategory { get; } = new List<FlatCalculationsVm>();

        public List<FlatCalculationsVm> AllFlatsByCategory { get; } = new List<FlatCalculationsVm>();

        public FlatOutput(List<FlatCalculationsVm> privateFlatsByCategory = null, List<FlatCalculationsVm> allFlatsByCategory = null)
        {
            if(privateFlatsByCategory != null)
                PrivateFlatsByCategory = privateFlatsByCategory;
            if(allFlatsByCategory != null)
                AllFlatsByCategory = allFlatsByCategory;
        }
    }
}
