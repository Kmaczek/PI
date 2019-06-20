using ConsoleTesting;
using Hangfire;
using Hangfire.MemoryStorage;
using System;

namespace Jobs.Scheduler
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Scheduler");

            GlobalConfiguration.Configuration.UseMemoryStorage();

            RecurringJob.AddOrUpdate(() => EmailSummaryJob.EmailJob(), "1/2 * * * *");
            Console.WriteLine("Email Job Enqueued.");

            Console.ReadLine();
        }
    }
}
