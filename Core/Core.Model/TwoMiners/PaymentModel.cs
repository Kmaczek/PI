using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model.TwoMiners
{
    public class PaymentModel
    {
        public decimal Amount { get; set; }
        public string Timestamp { get; set; }
        public string Tx { get; set; }
    }
}
