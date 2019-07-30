using Autofac;
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
    public static class ConsoleCommander
    {
        public static Dictionary<string, Action<string[]>> Commands = new Dictionary<string, Action<string[]>>();
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static ConsoleCommander()
        {
            Commands.Add("quit", Quit);
            Commands.Add("q", Quit);
            Commands.Add("exit", Quit);
            Commands.Add("p", Print);
            Commands.Add("job", RunJob);
        }

        public static void ListenToCommands(string command)
        {
            try
            {
                var splitted = command.Split(" ");

                if(Commands.ContainsKey(splitted[0]))
                    Commands[splitted[0]](splitted);
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

        public static void Quit(string[] command)
        {
            Log.Info("Quitting Scheduler...");
            Thread.Sleep(1000);
            Environment.Exit(0);
        }

        public static void Print(string[] command)
        {
            string message = "";
            if (command.Length > 1 && !string.IsNullOrWhiteSpace(command[1]))
            {
                message = string.Join(" ", command.Skip(1));
            }
            
            Log.Info($"Printing [{message}]");
        }


        public static Dictionary<string, IJob> JobsToRun = new Dictionary<string, IJob>()
        {
            {"es", Program.InjectionContainer.ResolveNamed<IJob>(nameof(EmailSummaryJob)) },
            {"EmailSummary", Program.InjectionContainer.ResolveNamed<IJob>(nameof(EmailSummaryJob)) },
            {"pc", Program.InjectionContainer.ResolveNamed<IJob>(nameof(PerformanceAuditJob)) },
            {"PerformanceCounterJob", Program.InjectionContainer.ResolveNamed<IJob>(nameof(PerformanceAuditJob)) },
        };
        public static void RunJob(string[] command)
        {
            if (command.Length > 1 && !string.IsNullOrWhiteSpace(command[1]))
            {
                if(command[1] == "list")
                {
                    var sb = new StringBuilder();
                    sb.AppendLine();

                    foreach(var key in JobsToRun.Keys)
                        sb.AppendLine($"\t-[{key}]");

                    Log.Info($"{sb}");

                    return;
                }

                if (JobsToRun.ContainsKey(command[1]))
                    JobsToRun[command[1]].ImmediateRun();
                else
                {
                    Log.Info($"Job [{command[1]}] is not registered");
                }
            }
        }
    }
}
