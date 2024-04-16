using System;
using System.Collections.Generic;

namespace Data.EF.Models
{
    public partial class Parser
    {
        public Parser()
        {
            PriceSeries = new HashSet<PriceSeries>();
        }

        public int Id { get; set; }
        public string Uri { get; set; }
        public int ParserTypeId { get; set; }
        public string Params { get; set; }
        public bool Track { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime ActiveFrom { get; set; }
        public DateTime ActiveTo { get; set; }
        public int LatestPriceDetailId { get; set; }

        public virtual ParserType ParserType { get; set; }
        public virtual ICollection<PriceSeries> PriceSeries { get; set; }
        public virtual PriceDetails LatestPriceDetail { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, " +
                   $"Uri: {Uri}, " +
                   $"ParserTypeId: {ParserTypeId}, " +
                   $"Params: {Params}, " +
                   $"Track: {Track}, " +
                   $"CreatedDate: {CreatedDate}, " +
                   $"UpdatedDate: {UpdatedDate}, " +
                   $"ActiveFrom: {ActiveFrom}, " +
                   $"ActiveTo: {ActiveTo}, " +
                   $"LatestPriceDetailId: {LatestPriceDetailId}";
        }
    }
}
