using System;
using System.Collections.Generic;

namespace Data.EF.Models
{
    public partial class FlatSeries
    {
        public FlatSeries()
        {
            DateFetched = DateTime.Now;
        }

        public int Id { get; set; }
        public DateTime DateFetched { get; set; }
        public decimal AvgPricePerMeter { get; set; }
        public decimal AvgPrice { get; set; }
        public int? CategoryId { get; set; }
        public int Amount { get; set; }
        public int? BiggestId { get; set; }
        public int? SmallestId { get; set; }
        public int? CheapestId { get; set; }
        public int? MostExpensiveId { get; set; }
        public int? BestValueId { get; set; }

        public virtual Flat BestValue { get; set; }
        public virtual Flat Biggest { get; set; }
        public virtual FlatCategoty Category { get; set; }
        public virtual Flat Cheapest { get; set; }
        public virtual Flat MostExpensive { get; set; }
        public virtual Flat Smallest { get; set; }
    }
}
