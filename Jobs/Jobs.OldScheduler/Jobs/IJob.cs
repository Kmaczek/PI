using System.Collections.Generic;

namespace Jobs.OldScheduler.Jobs
{
    public interface IJob
    {
        string JobName { get; }
        void Run();
        void ImmediateRun();
        void ImmediateRun(IEnumerable<string> parameters);
    }
}