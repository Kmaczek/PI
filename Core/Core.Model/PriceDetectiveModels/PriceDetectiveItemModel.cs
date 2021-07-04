using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model.PriceDetectiveModels
{
    public class PriceDetectiveItemModel
    {
        public string Title { get; set; }
        public decimal? Min { get; set; }
        public decimal? Max { get; set; }
        public decimal? Prev { get; set; }
        public decimal? Currrent { get; set; }
        public string Url { get; set; }
    }
}
