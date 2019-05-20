using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model
{
    public class XtbOutput
    {
        public double? Balance { get; set;}
        public double? Margin { get; set; }
        public double? MarginFree { get; set; }
        public double? MarginLevel { get; set; }
        public double? Equity { get; set; }
        public double? Credit { get; set; }

        public double? Gain => Equity - Balance;
    }
}
