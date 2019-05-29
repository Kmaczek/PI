namespace Jobs.OldScheduler
{
    public interface IJob
    {
        void Run();
        void ImmediateRun();
    }
}