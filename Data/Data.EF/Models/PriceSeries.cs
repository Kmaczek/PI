using System;
using System.Collections.Generic;

namespace Data.EF.Models
{
    public partial class PriceSeries
    {
        public int Id { get; set; }
        public int ParserId { get; set; }
        public int PriceDetailsId { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Product Parser { get; set; }
        public virtual PriceDetails PriceDetails { get; set; }
    }
}
