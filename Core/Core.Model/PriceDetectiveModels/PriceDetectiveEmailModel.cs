using System.Collections.Generic;

namespace Core.Model.PriceDetectiveModels
{
    public class PriceDetectiveEmailModel
    {
        List<PriceDetectiveItemModel> items = new List<PriceDetectiveItemModel>();
        public List<PriceDetectiveItemModel> Items
        {
            get { return items; }
            set
            {
                items.Clear();
                items.AddRange(value);
            }
        }
    }
}
