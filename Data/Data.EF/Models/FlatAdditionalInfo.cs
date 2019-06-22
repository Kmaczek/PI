using System;
using System.Collections.Generic;

namespace Data.EF.Models
{
    public partial class FlatAdditionalInfo
    {
        public int Id { get; set; }
        public int FlatId { get; set; }
        public int AdditionalInfoId { get; set; }

        public virtual AdditionalInfo AdditionalInfo { get; set; }
        public virtual Flat Flat { get; set; }
    }
}
