using System;
using System.Collections.Generic;

namespace Data.EF.Models
{
    public partial class AdditionalInfo
    {
        public AdditionalInfo()
        {
            FlatAdditionalInfo = new HashSet<FlatAdditionalInfo>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<FlatAdditionalInfo> FlatAdditionalInfo { get; set; }
    }
}
