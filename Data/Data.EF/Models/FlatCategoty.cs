using System;
using System.Collections.Generic;

namespace Data.EF.Models
{
    public partial class FlatCategoty
    {
        public FlatCategoty()
        {
            FlatSeries = new HashSet<FlatSeries>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<FlatSeries> FlatSeries { get; set; }
    }
}
