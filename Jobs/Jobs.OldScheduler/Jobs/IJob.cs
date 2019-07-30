namespace Jobs.OldScheduler.Jobs
{
    public interface IJob
    {
        string JobName { get; }
        void Run();
        void ImmediateRun();
    }
}