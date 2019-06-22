using System;
using System.Collections.Generic;

namespace Data.EF.Models
{
    public partial class SeriesParent
    {
        public SeriesParent()
        {
            Series = new HashSet<Series>();
        }

        public int Id { get; set; }
        public DateTime FetchedDate { get; set; }
        public decimal Total { get; set; }

        public virtual ICollection<Series> Series { get; set; }
    }
}
