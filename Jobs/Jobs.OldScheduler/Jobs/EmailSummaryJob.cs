using AutoMapper;
using Core.Common;
using Core.Domain.Logic;
using Core.Domain.Logic.EmailGeneration;
using Core.Model;
using Core.Model.FlatsModels;
using Data.Repository.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Xtb.Core;

namespace Jobs.OldScheduler.Jobs
{
    public class EmailSummaryJob : IJob
    {
        private static Timer RunningTimer = null;
        private IConfigurationRoot configuration;
        private XtbInterface xtbService;
        private readonly IBinanceService binanceService;
        private readonly IEmailService emailService;
        private readonly IOtoDomRepository otoDomRepository;
        private readonly ILogger Log;
        private readonly IMapper mapper;

        public string JobName => nameof(EmailSummaryJob);

        public EmailSummaryJob(
            IConfigurationRoot configuration,
            ILogger log,
            IMapper mapper,
            XtbInterface xtbService,
            IBinanceService binanceService,
            IEmailService emailService,
            IOtoDomRepository otoDomRepository)
        {
            this.configuration = configuration;
            this.Log = log;
            this.mapper = mapper;
            this.xtbService = xtbService;
            this.binanceService = binanceService;
            this.emailService = emailService;
            this.otoDomRepository = otoDomRepository;
        }

        public void Run()
        {
            var hour = Convert.ToInt32(configuration.GetSection("emailJobHour").Value, CultureInfo.InvariantCulture);
            var minute = Convert.ToInt32(configuration.GetSection("emailJobMinute").Value, CultureInfo.InvariantCulture);

            var configDate = DateTime.Now.Date + new TimeSpan(
                hour,
                minute,
                0);

            Log.Info($"Config for {JobName}. (H:{hour} m:{minute}). Job will run daily at: {configDate.ToShortTimeString()}");

            var date = DateTime.Now.AddDays(1);
            TimeSpan startTimeSpan;
            if (configDate > DateTime.Now)
            {
                startTimeSpan = new TimeSpan(configDate.Ticks - DateTime.Now.Ticks);
            }
            else
            {
                startTimeSpan = new TimeSpan(configDate.AddDays(1).Ticks - DateTime.Now.Ticks);
            }

            var periodTimeSpan = TimeSpan.FromDays(1);

            RunningTimer = new Timer((e) =>
            {
                EmailJob();
                Log.Info($"Next Run of {JobName}, in {periodTimeSpan}.");
            }, null, startTimeSpan, periodTimeSpan);

            Log.Info($"Enqueued Email Summary job, first run will start in {startTimeSpan}.");
        }

        public void ImmediateRun()
        {
            Log.Info($"Immediate execution of {JobName}.");
            EmailJob();
            Log.Info($"Job {JobName} done.");
        }

        public void EmailJob()
        {
            Log.Info($"Processing Email Summary job, time {DateTime.Now.ToLocalTime()}.");

            // Create service that provides generators
            XtbHtmlGenerator xtbHtmlGenerator = GetXtbGenerator();
            OtodomHtmlGenerator otodomHtmlGenerator = GetOtodomGenerator();
            BinanceHtmlGenerator binanceGenerator = GetBinanceGenerator();

            EmailAssembler emailAssembler = new EmailAssembler(
                new List<IHtmlGenerator>
                {
                    xtbHtmlGenerator,
                    otodomHtmlGenerator,
                    binanceGenerator
                });

            emailService.SendEmail(emailAssembler.GenerateEmail());
            Log.Info($"Email Summary job finished, time {DateTime.Now.ToLocalTime()}.");
        }

        private BinanceHtmlGenerator GetBinanceGenerator()
        {
            var symbolValues = binanceService.GetSymbolValuesForAccount();
            var binanceGenerator = new BinanceHtmlGenerator(symbolValues);
            return binanceGenerator;
        }

        private XtbHtmlGenerator GetXtbGenerator()
        {
            var balance = xtbService.GetBalance();
            XtbOutput balanceOutput = mapper.Map<XtbOutput>(balance);
            var xtbHtmlGenerator = new XtbHtmlGenerator(balanceOutput);
            return xtbHtmlGenerator;
        }

        private OtodomHtmlGenerator GetOtodomGenerator()
        {
            var privateOffers = otoDomRepository.GetPrivateFlats();
            var mappedPrivate = MapToFlatsBM(privateOffers);
            var privateFlatAggregate = new FlatAggregateVM(mappedPrivate);

            var allOffers = otoDomRepository.GetActiveFlats();
            var flatDataBMs = MapToFlatsBM(allOffers);
            var allFlatAggregate = new FlatAggregateVM(flatDataBMs);

            var flatsOutput = new FlatOutput(privateFlatAggregate.FlatCalculations, allFlatAggregate.FlatCalculations);
            var otodomHtmlGenerator = new OtodomHtmlGenerator(flatsOutput);
            return otodomHtmlGenerator;
        }

        private List<FlatDataBM> MapToFlatsBM(IEnumerable<Data.EF.Models.Flat> allOffers)
        {
            var flatDataBMs = new List<FlatDataBM>();
            foreach (var flat in allOffers)
            {
                var flatBM = new FlatDataBM(
                    flat.Surface,
                    flat.TotalPrice,
                    flat.Rooms.HasValue ? (int)flat.Rooms : 0,
                    flat.Url,
                    flat.IsPrivate.HasValue ? flat.IsPrivate.Value : false);
                flatDataBMs.Add(flatBM);
            }

            return flatDataBMs;
        }

        private void LogFlatScrappingErrors(IEnumerable<FlatDataBM> flats)
        {
            var flatsWithErrors = flats.Where(x => x.Errors.Any());
            if (flatsWithErrors.Any())
            {
                Log.Info($"Detected {flatsWithErrors.Count()} flats with errors. Printing them now.");

                foreach (var flat in flatsWithErrors)
                {
                    var joinedErrors = string.Join(Environment.NewLine, flat.Errors);
                    Log.Info($"Flat errors: {Environment.NewLine}{joinedErrors}.");
                }
            }
        }
    }
}
