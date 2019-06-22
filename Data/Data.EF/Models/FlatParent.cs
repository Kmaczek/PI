using System;
using System.Collections.Generic;

namespace Data.EF.Models
{
    public partial class FlatParent
    {
        public int Id { get; set; }
        public DateTime DateFetched { get; set; }
        public decimal? AvgPricePerMeter { get; set; }
        public decimal? AvgPrice { get; set; }
    }
}
