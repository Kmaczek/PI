using Autofac;
using AutoMapper;
using Binance.Api;
using Common;
using Core.Common;
using Core.Domain.Logic;
using Core.Model;
using log4net;
using log4net.Config;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Reflection;
using xAPI.Records;
using Xtb.Core;

namespace Jobs.OldScheduler
{
    internal class Program
    {
        public static IConfigurationRoot Configuration;
        public static IContainer InjectionContainer;
        public delegate void CommandDelegate(string command);
        public static event CommandDelegate OnCommandReceived;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static void Main()
        {
            SetupLogger();

            Log.Info("Scheduler started...");

            AutomapServiceBehavior.InitializeMapper();
            AppDomain.CurrentDomain.UnhandledException += HandleException;
            OnCommandReceived += ConsoleCommander.ListenToCommands;

            SetupConfig();
            BuildInjectionContainer();

            var emailJob = InjectionContainer.ResolveNamed<IJob>(nameof(EmailSummaryJob));
            emailJob.Run();
            var auditJob = InjectionContainer.ResolveNamed<IJob>(nameof(PerformanceAuditJob));
            auditJob.Run();

            while (true)
            {
                var command = Console.ReadLine();
                OnCommandReceived(command);
            }
        }

        private static void SetupLogger()
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }

        private static void SetupConfig()
        {
            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appSettings.json", optional: true, reloadOnChange: true)
                            .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        private static void BuildInjectionContainer()
        {
            var diBuilder = new ContainerBuilder();
            diBuilder.RegisterInstance(Configuration).SingleInstance();
            diBuilder.RegisterModule<LoggingModule>();
            diBuilder.RegisterType<XtbService>().As<XtbInterface>();
            diBuilder.RegisterType<BinanceClient>().As<BinanceClientInterface>();
            diBuilder.RegisterType<BinanceService>().As<IBinanceService>();
            diBuilder.RegisterType<PerformanceAudit>().As<IPerformanceAudit>();
            diBuilder.RegisterType<EmailSummaryJob>().Named<IJob>(nameof(EmailSummaryJob));
            diBuilder.RegisterType<PerformanceAuditJob>().Named<IJob>(nameof(PerformanceAuditJob));
            diBuilder.RegisterType<EmailService>().As<IEmailService>(); 
            InjectionContainer = diBuilder.Build();
        }

        public static void HandleException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Error(e.ExceptionObject.ToString(), sender as Exception);
        }

        public sealed class AutomapServiceBehavior
        {
            static bool _initialized;

            public static void InitializeMapper()
            {
                if (_initialized)
                    return;

                Mapper.Initialize(cfg => cfg.CreateMap<StreamingBalanceRecord, XtbOutput>());
                _initialized = true;
            }
        }
    }
}
