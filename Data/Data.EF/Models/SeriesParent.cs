using System;
using System.Collections.Generic;

namespace Data.EF.Models
{
    public partial class SeriesParent
    {
        public SeriesParent()
        {
            Series = new HashSet<BinanceSeries>();
        }

        public int Id { get; set; }
        public DateTime FetchedDate { get; set; }
        public decimal Total { get; set; }

        public virtual ICollection<BinanceSeries> Series { get; set; }
    }
}
