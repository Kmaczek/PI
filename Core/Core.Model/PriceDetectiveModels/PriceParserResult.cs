using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model.PriceDetectiveModels
{
    public class PriceParserResult
    {
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string RetailerNo { get; set; }
        public int ParserConfigId { get; set; }
        public bool Proper { get; set; }
    }
}
