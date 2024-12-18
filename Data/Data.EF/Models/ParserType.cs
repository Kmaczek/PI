using System;
using System.Collections.Generic;

namespace Data.EF.Models
{
    public partial class ParserType
    {
        public ParserType()
        {
            Parser = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }

        public virtual ICollection<Product> Parser { get; set; }
    }
}
