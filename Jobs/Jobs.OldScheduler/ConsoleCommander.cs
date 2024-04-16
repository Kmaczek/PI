using Autofac;
using Data.Repository;
using Data.Repository.Interfaces;
using Jobs.OldScheduler.Jobs;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Jobs.OldScheduler
{
    public class ConsoleCommander
    {
        public static Dictionary<string, Action<string[]>> Commands = new Dictionary<string, Action<string[]>>();
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IJob emailSummaryJob;
        private readonly IJob performanceAuditJob;
        private readonly IJob otodomFeedJob;
        private readonly IJob priceDetectiveJob;
        private readonly ILifetimeScope scope;
        private readonly IPriceRepository priceRepository;
        private Dictionary<string, IJob> JobsToRun = new Dictionary<string, IJob>();

        public ConsoleCommander(ILifetimeScope scope, IPriceRepository priceRepository)
        {
            this.scope = scope;
            this.priceRepository = priceRepository;
            emailSummaryJob = scope.ResolveNamed<IJob>(nameof(EmailSummaryJob));
            performanceAuditJob = scope.ResolveNamed<IJob>(nameof(PerformanceAuditJob));
            otodomFeedJob = scope.ResolveNamed<IJob>(nameof(OtodomFeedJob));
            priceDetectiveJob = scope.ResolveNamed<IJob>(nameof(PriceDetectiveJob));

            SetJobsToRun();
            AddCommands();
        }

        private void SetJobsToRun()
        {
            JobsToRun.Add("es", emailSummaryJob);
            JobsToRun.Add("EmailSummary", emailSummaryJob);

            JobsToRun.Add("pa", performanceAuditJob);
            JobsToRun.Add("PerformanceAuditJob", performanceAuditJob);

            JobsToRun.Add("of", otodomFeedJob);
            JobsToRun.Add("OtodomFeedJob", otodomFeedJob);

            JobsToRun.Add("pd", priceDetectiveJob);
            JobsToRun.Add(nameof(PriceDetectiveJob), priceDetectiveJob);
        }

        private void AddCommands()
        {
            Commands.Add("quit", Quit);
            Commands.Add("q", Quit);
            Commands.Add("exit", Quit);
            Commands.Add("p", Print);
            Commands.Add("job", RunJob);
            Commands.Add("list", List);
        }

        public void ListenToCommands(string command)
        {
            try
            {
                var splitted = command.Split(" ");

                if (Commands.ContainsKey(splitted[0]))
                {
                    Commands[splitted[0]](splitted);
                }
                else
                {
                    Log.Info($"Command [{splitted[0]}] is incorrect");
                }
            }
            catch (Exception e)
            {
                Log.Error("Unhandled error", e);
            }
        }

        private void Quit(string[] command)
        {
            Log.Info("Quitting Scheduler...");
            Thread.Sleep(1000);
            Environment.Exit(0);
        }

        private void Print(string[] command)
        {
            string message = "";
            if (command.Length > 1 && !string.IsNullOrWhiteSpace(command[1]))
            {
                message = string.Join(" ", command.Skip(1));
            }

            Log.Info($"Printing [{message}]");
        }

        private void List(string[] command)
        {
            if (command.Contains("jobs"))
            {
                Log.Info($"Listing Jobs");
                priceRepository.GetParsers().ToList().ForEach(x => Log.Info(x.ToString()));
            }
        }

        private void RunJob(string[] command)
        {
            if (command.Length > 1 && !string.IsNullOrWhiteSpace(command[1]))
            {
                if (command[1] == "list")
                {
                    var sb = new StringBuilder();
                    sb.AppendLine();

                    foreach (var key in JobsToRun.Keys)
                    {
                        sb.AppendLine($"\t-[{key}]");
                    }

                    Log.Info($"{sb}");

                    return;
                }

                if (JobsToRun.ContainsKey(command[1]))
                {
                    if(command.Length > 2)
                    {
                        JobsToRun[command[1]].ImmediateRun(command.Skip(2));
                    }
                    else
                    {
                        JobsToRun[command[1]].ImmediateRun();
                    }
                }
                else
                {
                    Log.Info($"Job [{command[1]}] is not registered");
                }
            }
        }
    }
}
