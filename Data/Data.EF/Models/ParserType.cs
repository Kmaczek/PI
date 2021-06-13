using System;
using System.Collections.Generic;

namespace Data.EF.Models
{
    public partial class ParserType
    {
        public ParserType()
        {
            Parser = new HashSet<Parser>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Parser> Parser { get; set; }
    }
}
