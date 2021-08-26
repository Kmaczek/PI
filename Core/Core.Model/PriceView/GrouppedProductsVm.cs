using System.Collections.Generic;

namespace Core.Model.PriceView
{
    public class GrouppedProductsVm
    {
        public int SiteId { get; set; }
        public string SiteName { get; set; }
        public IEnumerable<ProductVm> Products { get; set; }
    }
}
