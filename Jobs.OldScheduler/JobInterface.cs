namespace Jobs.OldScheduler
{
    public interface JobInterface
    {
        void Run();
        void ImmediateRun();
    }
}