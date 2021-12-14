using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model.TwoMiners
{
    public class RewardModel
    {
        public int Blockheight { get; set; }
        public string Timestamp { get; set; }
        public string Blockhash { get; set; }
        public int Reward { get; set; }
        public decimal Percent { get; set; }
        public bool Immature { get; set; }
        public decimal CurrentLuck { get; set; }
        public bool Uncle { get; set; }
    }
}
