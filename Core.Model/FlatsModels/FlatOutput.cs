using System.Collections.Generic;

namespace Core.Model.FlatsModels
{
    public class FlatOutput
    {
        public List<FlatCalculationsVM> PrivateFlatsByCategory { get; set; } = new List<FlatCalculationsVM>();

        public List<FlatCalculationsVM> AllFlatsByCategory { get; set; } = new List<FlatCalculationsVM>();
    }
}
