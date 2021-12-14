using System.Collections.Generic;

namespace Core.Model.TwoMiners
{
    public class AccountModel
    {
        public int ApiVersion { get; set; }
        public IEnumerable<ConfigModel> Config { get; set; }
        public decimal CurrentHashrate { get; set; }
        public string CurrentLuck { get; set; }
        public decimal Hashrate { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<PaymentModel> Payments { get; set; }
        public int PaymentsTotal { get; set; }
        public IEnumerable<RewardModel> Rewards { get; set; }
        public int RoundShares { get; set; }
        public int SharesInvalid { get; set; }
        public int SharesStale { get; set; }
        public int SharesValid { get; set; }
        public StatsModel Stats { get; set; }
        public IEnumerable<SumRewardModel> Sumrewards { get; set; }
        public string UpdatedAt { get; set; }
        public IDictionary<string, WorkerModel> Workers { get; set; }
        public int WorkersOffline { get; set; }
        public int WorkersOnline { get; set; }
        public int WorkersTotal { get; set; }

    }
}
