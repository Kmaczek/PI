using Data.EF.Models;
using System;

namespace Core.Model.PriceView
{
    public class ProductVm
    {
        public int Id { get; set; }
        public string Uri { get; set; }
        public int ParserTypeId { get; set; }
        public string Params { get; set; }
        public bool Track { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime ActiveFrom { get; set; }
        public DateTime ActiveTo { get; set; }
        public int Category { get; set; }

        public int? LatestPriceDetailId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public static ProductVm FromDm(Product productDm)
        {
            if (productDm == null) return null;

            return new ProductVm
            {
                Id = productDm.Id,
                Uri = productDm.Uri,
                ParserTypeId = productDm.ParserTypeId,
                Params = productDm.Params,
                Track = productDm.Track,
                CreatedDate = productDm.CreatedDate,
                UpdatedDate = productDm.UpdatedDate,
                ActiveFrom = productDm.ActiveFrom,
                ActiveTo = productDm.ActiveTo,
                Category = productDm.Category,
                LatestPriceDetailId = productDm.LatestPriceDetailId,
                Name = productDm.LatestPriceDetail?.Title,
                Code = productDm.LatestPriceDetail?.RetailerNo 
            };
        }
    }
}
