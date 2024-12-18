using System;

namespace Pi.Api.EF.Models.Price
{
    public partial class PriceDetailsDm
    {
        public PriceDetailsDm()
        {
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string RetailerNo { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
