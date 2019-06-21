using AutoMapper;
using Core.Common;
using Core.Domain.Logic;
using Core.Domain.Logic.EmailGeneration;
using Core.Model;
using Core.Model.FlatsModels;
using Flats.Core.Scraping;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Xtb.Core;

namespace Jobs.OldScheduler
{
    public class EmailSummaryJob : IJob
    {
        private static Timer RunningTimer = null;
        private IConfigurationRoot _configuration;
        private XtbInterface _xtbService;
        private readonly IBinanceService _binanceService;
        private readonly IEmailService _emailService;
        private readonly ILogger Log;

        public EmailSummaryJob(
            IConfigurationRoot configuration, 
            ILogger log,
            XtbInterface xtbService,
            IBinanceService binanceService,
            IEmailService emailService)
        {
            this._configuration = configuration;
            this.Log = log;
            this._xtbService = xtbService;
            this._binanceService = binanceService;
            this._emailService = emailService;
        }

        public void Run()
        {
            var hour = Convert.ToInt32(_configuration.GetSection("emailJobHour").Value, CultureInfo.InvariantCulture);
            var minute = Convert.ToInt32(_configuration.GetSection("emailJobMinute").Value, CultureInfo.InvariantCulture);
            
            var configDate = DateTime.Now.Date + new TimeSpan(
                hour,
                minute, 
                0);

            Log.Info($"Config for {nameof(EmailSummaryJob)}. (H:{hour} m:{minute}). Job will run daily at: {configDate.ToShortTimeString()}");

            var date = DateTime.Now.AddDays(1);
            TimeSpan startTimeSpan;
            if(configDate > DateTime.Now)
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
                Log.Info($"Next Run of {nameof(EmailSummaryJob)}, in {periodTimeSpan}.");
            }, null, startTimeSpan, periodTimeSpan);

            Log.Info($"Enqueued Email Summary job, first run will start in {startTimeSpan}.");
        }

        public void ImmediateRun()
        {
            Log.Info($"Immediate execution of {nameof(EmailSummaryJob)}.");
            EmailJob();
            Log.Info($"Job {nameof(EmailSummaryJob)} done.");
        }

        public void EmailJob()
        {
            Log.Info($"Processing Email Summary job, time {DateTime.Now.ToLocalTime()}.");

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

            _emailService.SendEmail(emailAssembler.GenerateEmail());
            Log.Info($"Email Summary job finished, time {DateTime.Now.ToLocalTime()}.");
        }

        private BinanceHtmlGenerator GetBinanceGenerator()
        {
            var symbolValues = _binanceService.GetSymbolValuesForAccount();
            var binanceGenerator = new BinanceHtmlGenerator(symbolValues);
            return binanceGenerator;
        }

        private XtbHtmlGenerator GetXtbGenerator()
        {
            var balance = _xtbService.GetBalance();
            XtbOutput balanceOutput = Mapper.Map<XtbOutput>(balance);
            var xtbHtmlGenerator = new XtbHtmlGenerator(balanceOutput);
            return xtbHtmlGenerator;
        }

        private OtodomHtmlGenerator GetOtodomGenerator()
        {
            var otoDomScrapper = new OtoDomScrapper();
            var privateScrapeResult = otoDomScrapper.Scrape();
            LogFlatScrappingErrors(privateScrapeResult);
            var privateFlatAggregate = new FlatAggregateVM(privateScrapeResult);

            otoDomScrapper.ScrapingUrl = OtoDomScrapper.AllOffers;
            var allScrapeResult = otoDomScrapper.Scrape();
            LogFlatScrappingErrors(allScrapeResult);
            var allFlatAggregate = new FlatAggregateVM(allScrapeResult);

            var flatsOutput = new FlatOutput(privateFlatAggregate.FlatCalculations, allFlatAggregate.FlatCalculations);
            var otodomHtmlGenerator = new OtodomHtmlGenerator(flatsOutput);
            return otodomHtmlGenerator;
        }

        private void LogFlatScrappingErrors(IEnumerable<FlatDataBM> flats)
        {
            var flatsWithErrors = flats.Where(x => x.Errors.Any());
            if(flatsWithErrors.Any())
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
