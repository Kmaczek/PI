using System;
using System.Collections.Generic;

namespace Data.EF.Models
{
    public partial class Flat
    {
        private const string otodomBaseUrl = "https://www.otodom.pl";
        private string normalizedUrl = null;
        public Flat()
        {
            FlatAdditionalInfo = new HashSet<FlatAdditionalInfo>();
            FlatSeriesBestValue = new HashSet<FlatSeries>();
            FlatSeriesBiggest = new HashSet<FlatSeries>();
            FlatSeriesCheapest = new HashSet<FlatSeries>();
            FlatSeriesMostExpensive = new HashSet<FlatSeries>();
            FlatSeriesSmallest = new HashSet<FlatSeries>();
        }

        public int Id { get; set; }
        public string OtoDomId { get; set; }
        public decimal Surface { get; set; }
        public decimal TotalPrice { get; set; }
        public byte? Rooms { get; set; }
        public bool? IsPrivate { get; set; }
        public decimal? Rent { get; set; }
        public byte? Floor { get; set; }
        public byte? FloorsNo { get; set; }
        public short? ConstructionYear { get; set; }
        public int? MarketId { get; set; }
        public int? HeatingId { get; set; }
        public int? LocationId { get; set; }
        public int? FormOfPropertyId { get; set; }
        public int? TypeId { get; set; }
        public string Url { get; set; }
        public string NormalizedUrl 
        { 
            get
            {
                if(normalizedUrl == null)
                {
                    if (Url.Contains("https"))
                        normalizedUrl = Url;
                    else
                        normalizedUrl = otodomBaseUrl + Url;
                }

                return normalizedUrl;
            }
        }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual FormOfProperty FormOfProperty { get; set; }
        public virtual Heating Heating { get; set; }
        public virtual Location Location { get; set; }
        public virtual Market Market { get; set; }
        public virtual TypeOfBuilding Type { get; set; }
        public virtual ICollection<FlatAdditionalInfo> FlatAdditionalInfo { get; set; }
        public virtual ICollection<FlatSeries> FlatSeriesBestValue { get; set; }
        public virtual ICollection<FlatSeries> FlatSeriesBiggest { get; set; }
        public virtual ICollection<FlatSeries> FlatSeriesCheapest { get; set; }
        public virtual ICollection<FlatSeries> FlatSeriesMostExpensive { get; set; }
        public virtual ICollection<FlatSeries> FlatSeriesSmallest { get; set; }
    }
}
