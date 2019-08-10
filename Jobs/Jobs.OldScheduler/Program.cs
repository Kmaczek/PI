using Autofac;
using AutoMapper;
using AutoMapper.Configuration;
using Binance.Api;
using Common;
using Core.Common;
using Core.Domain.Logic;
using Core.Domain.Logic.FlatsFeed;
using Core.Domain.Logic.OtodomService;
using Core.Model;
using Core.Model.FlatsModels;
using Data.EF.Models;
using Data.Repository;
using Data.Repository.Interfaces;
using Flats.Core.Scraping;
using Jobs.OldScheduler.Jobs;
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
        public delegate void CommandDelegate(string command);
        public static event CommandDelegate OnCommandReceived;

        private static ConsoleCommander consoleCommander;
        private static IConfigurationRoot configuration;
        private static IMapper mapper;
        private static IContainer injectionContainer;
        
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static void Main()
        {
            SetupLogger();

            Log.Info("Scheduler started...");

            mapper = CreateMapper();
            AppDomain.CurrentDomain.UnhandledException += HandleException;
            
            SetupConfig();
            BuildInjectionContainer();

            consoleCommander = injectionContainer.Resolve<ConsoleCommander>();
            OnCommandReceived += consoleCommander.ListenToCommands;

            //RunTestMethod();

            //TODO: Make it configurable
            var emailJob = injectionContainer.ResolveNamed<IJob>(nameof(EmailSummaryJob));
            emailJob.Run();
            var auditJob = injectionContainer.ResolveNamed<IJob>(nameof(PerformanceAuditJob));
            //auditJob.Run();
            var flatsFeedingJob = injectionContainer.ResolveNamed<IJob>(nameof(OtodomFeedJob));
            //flatsFeedingJob.ImmediateRun();
            flatsFeedingJob.Run();

            while (true)
            {
                var command = Console.ReadLine();
                OnCommandReceived(command);
            }
        }

        private static void RunTestMethod()
        {
            var otoDomScrapper = injectionContainer.Resolve<Scraper>() as OtoDomScrapper;
            otoDomScrapper.ScrapingUrl = otoDomScrapper.AllOffers;
            var s = otoDomScrapper.Scrape();
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
            configuration = builder.Build();
        }

        private static void BuildInjectionContainer()
        {
            var diBuilder = new ContainerBuilder();
            diBuilder.RegisterInstance(configuration).SingleInstance();
            diBuilder.RegisterInstance(mapper).SingleInstance();
            diBuilder.RegisterModule<LoggingModule>();

            diBuilder.RegisterType<PiContext>();
            diBuilder.Register<Func<PiContext>>(x => 
            {
                var context = x.Resolve<IComponentContext>();
                return () => context.Resolve<PiContext>();
            });
            diBuilder.RegisterType<BinanceRepository>().As<IBinanceRepository>();
            diBuilder.RegisterType<OtoDomRepository>().As<IOtoDomRepository>();

            diBuilder.RegisterType<OtoDomScrapper>().As<IScrapper>();
            diBuilder.RegisterType<XtbService>().As<XtbInterface>();
            diBuilder.RegisterType<BinanceClient>().As<IBinanceClient>();
            diBuilder.RegisterType<BinanceService>().As<IBinanceService>();
            diBuilder.RegisterType<PerformanceAudit>().As<IPerformanceAudit>();
            diBuilder.RegisterType<EmailService>().As<IEmailService>();
            diBuilder.RegisterType<OtodomFeedService>().As<IFlatsFeedService>();

            //Jobs
            diBuilder.RegisterType<EmailSummaryJob>().Named<IJob>(nameof(EmailSummaryJob));
            diBuilder.RegisterType<PerformanceAuditJob>().Named<IJob>(nameof(PerformanceAuditJob));
            diBuilder.RegisterType<OtodomFeedJob>().Named<IJob>(nameof(OtodomFeedJob));

            diBuilder.RegisterType<ConsoleCommander>().SingleInstance();

            injectionContainer = diBuilder.Build();
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
                //TODO: remove global mapper in all places
                Mapper.Initialize(cfg => cfg.CreateMap<StreamingBalanceRecord, XtbOutput>());
                _initialized = true;
            }
        }

        public static IMapper CreateMapper()
        {
            var mapperInit = new MapperConfigurationExpression();
            mapperInit.CreateMap<StreamingBalanceRecord, XtbOutput>();
            mapperInit.CreateMap<FlatDataBM, Flat>();
            mapperInit.CreateMap<Flat, FlatDataBM>();

            var config = new MapperConfiguration(mapperInit);
            var mapper = config.CreateMapper();

            return mapper;
        }
    }
}
