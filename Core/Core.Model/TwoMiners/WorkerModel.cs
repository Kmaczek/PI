namespace Core.Model.TwoMiners
{
    public class WorkerModel
    {
        public string LastBeat { get; set; }
        public int Hr { get; set; }
        public bool Offline { get; set; }
        public int Hr2 { get; set; }
        public int SharesValid { get; set; }
        public int SharesInvalid { get; set; }
        public int SharesStale { get; set; }
    }
}
