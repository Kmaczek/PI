using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model.Inflation
{
    public class ProductGroup
    {
        public ProductGroup(Category category, decimal weight)
        {
            Category = category;
            Weight = weight;
        }

        public Category Category { get; set; }
        public decimal Weight { get; set; }
    }
}
