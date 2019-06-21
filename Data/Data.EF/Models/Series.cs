using System;
using System.Collections.Generic;

namespace Data.EF.Models
{
    public partial class BinanceSeries
    {
        public int Id { get; set; }
        public int SeriesParentId { get; set; }
        public string Currency { get; set; }
        public decimal Ammount { get; set; }
        public decimal AvgPrice { get; set; }

        public virtual SeriesParent SeriesParent { get; set; }
    }
}
