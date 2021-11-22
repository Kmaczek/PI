using Autofac;
using AutoMapper;
using AutoMapper.Configuration;
using Binance.Api;
using Common;
using Core.Common;
using Core.Common.Config;
using Core.Common.Logging;
using Core.Domain.Logic;
using Core.Domain.Logic.EmailGeneration;
using Core.Domain.Logic.FlatsFeed;
using Core.Domain.Logic.OtodomService;
using Core.Domain.Logic.PriceDetective;
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
using System.Linq;
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
        private static JobRunner jobRunner;

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static void Main()
        {
            SetupLogger();
            Log.Info("Scheduler started...");

            mapper = CreateMapper();
            AppDomain.CurrentDomain.UnhandledException += HandleException;

            SetupConfig();
            BuildInjectionContainer();

            AttachConsoleCommander();
            //RunTestMethod();

            //TODO: Make it configurable
            jobRunner = new JobRunner(injectionContainer);
            jobRunner.AddJob(nameof(EmailSummaryJob));
            jobRunner.AddJob(nameof(PerformanceAuditJob));
            jobRunner.AddJob(nameof(OtodomFeedJob));
            jobRunner.AddJob(nameof(PriceDetectiveJob));

            jobRunner.RunJobs();

            while (true)
            {
                var command = Console.ReadLine();
                OnCommandReceived?.Invoke(command);
            }
        }

        private static void AttachConsoleCommander()
        {
            consoleCommander = injectionContainer.Resolve<ConsoleCommander>();
            OnCommandReceived += consoleCommander.ListenToCommands;
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
            diBuilder.RegisterType<ConfigHelper>().SingleInstance();
            diBuilder.RegisterInstance(mapper).SingleInstance();
            diBuilder.RegisterModule<LoggingModule>();

            diBuilder.RegisterType<PiContext>();
            diBuilder.Register<Func<PiContext>>(x =>
            {
                var context = x.Resolve<IComponentContext>();
                return () => context.Resolve<PiContext>();
            });
            
            diBuilder.RegisterType<SettingRepository>().As<ISettingRepository>();
            diBuilder.RegisterType<BinanceRepository>().As<IBinanceRepository>();
            diBuilder.RegisterType<OtoDomRepository>().As<IOtoDomRepository>();
            diBuilder.RegisterType<PriceRepository>().As<IPriceRepository>();

            diBuilder.RegisterType<OtoDomScrapper>().As<IScrapper>();
            diBuilder.RegisterType<XtbService>().As<XtbInterface>();
            diBuilder.RegisterType<BinanceClient>().As<IBinanceClient>();
            diBuilder.RegisterType<BinanceService>().As<IBinanceService>();
            diBuilder.RegisterType<PerformanceAudit>().As<IPerformanceAudit>();
            diBuilder.RegisterType<EmailService>().As<IEmailService>();
            diBuilder.RegisterType<OtodomFeedService>().As<IFlatsFeedService>();
            diBuilder.RegisterType<PriceDetectiveService>().As<IPriceDetectiveService>();

            //Generators
            diBuilder.RegisterType<XtbHtmlGenerator>();
            diBuilder.Register<Func<XtbHtmlGenerator>>(x =>
            {
                var context = x.Resolve<IComponentContext>();
                return () => context.Resolve<XtbHtmlGenerator>();
            });
            diBuilder.RegisterType<BinanceHtmlGenerator>();
            diBuilder.Register<Func<BinanceHtmlGenerator>>(x =>
            {
                var context = x.Resolve<IComponentContext>();
                return () => context.Resolve<BinanceHtmlGenerator>();
            });
            diBuilder.RegisterType<OtodomHtmlGenerator>();
            diBuilder.Register<Func<OtodomHtmlGenerator>>(x =>
            {
                var context = x.Resolve<IComponentContext>();
                return () => context.Resolve<OtodomHtmlGenerator>();
            });
            diBuilder.RegisterType<PriceDetectiveGenerator>();
            diBuilder.Register<Func<PriceDetectiveGenerator>>(x =>
            {
                var context = x.Resolve<IComponentContext>();
                return () => context.Resolve<PriceDetectiveGenerator>();
            });
            diBuilder.RegisterType<EmailGeneratorFactory>().As<IEmailGeneratorFactory>();

            //Jobs
            diBuilder.RegisterType<EmailSummaryJob>().Named<IJob>(nameof(EmailSummaryJob));
            diBuilder.RegisterType<PerformanceAuditJob>().Named<IJob>(nameof(PerformanceAuditJob));
            diBuilder.RegisterType<OtodomFeedJob>().Named<IJob>(nameof(OtodomFeedJob));
            diBuilder.RegisterType<PriceDetectiveJob>().Named<IJob>(nameof(PriceDetectiveJob));

            diBuilder.RegisterType<ConsoleCommander>().SingleInstance();

            injectionContainer = diBuilder.Build();
            var errors = injectionContainer.Verify();
            if (errors.Any())
            {
                Log.Debug($"Found {errors.Count} errors in container.");
                foreach (var error in errors)
                {
                    Log.Debug(error);
                }
            }
        }

        public static void HandleException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Error(e.ExceptionObject.ToString(), sender as Exception);
        }

        public static IMapper CreateMapper()
        {
            var mapperInit = new MapperConfigurationExpression();
            mapperInit.CreateMap<StreamingBalanceRecord, XtbOutput>();
            mapperInit.CreateMap<FlatDataBm, Flat>();
            mapperInit.CreateMap<Flat, FlatDataBm>();

            var config = new MapperConfiguration(mapperInit);
            var mapper = config.CreateMapper();

            return mapper;
        }
    }
}
