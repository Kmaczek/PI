using System;
using System.Collections.Generic;

namespace Data.EF.Models
{
    public partial class PriceDetails
    {
        public PriceDetails()
        {
            PriceSeries = new HashSet<PriceSeries>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string RetailerNo { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual ICollection<PriceSeries> PriceSeries { get; set; }
    }
}
