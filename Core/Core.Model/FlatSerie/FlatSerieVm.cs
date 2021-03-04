using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model.FlatSerie
{
    public class FlatSerieVm
    {
        public FlatSerieVm()
        { }

        public int Id { get; set; }
        public DateTime Day { get; set; }
        public decimal AvgPricePerMeter { get; set; }
        public decimal AvgPrice { get; set; }
        public int Amount { get; set; }
    }
}
