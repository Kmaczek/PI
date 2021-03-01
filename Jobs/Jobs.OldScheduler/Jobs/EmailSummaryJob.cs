using AutoMapper;
using Core.Common;
using Core.Domain.Logic;
using Core.Domain.Logic.EmailGeneration;
using Core.Model;
using Core.Model.FlatsModels;
using Data.EF.Models;
using Data.Repository.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using MoreLinq;
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
        private readonly IEmailGeneratorFactory emailGeneratorFactory;
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
            IOtoDomRepository otoDomRepository,
            IEmailGeneratorFactory emailGeneratorFactory)
        {
            this.configuration = configuration;
            this.Log = log;
            this.mapper = mapper;
            this.xtbService = xtbService;
            this.binanceService = binanceService;
            this.emailService = emailService;
            this.otoDomRepository = otoDomRepository;
            this.emailGeneratorFactory = emailGeneratorFactory;
        }

        public void Run()
        {
            EmailJob();
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
            EmailAssembler emailAssembler = new EmailAssembler(
                new List<IHtmlGenerator>
                {
                    GetXtbGenerator(),
                    GetOtodomGenerator(),
                    GetBinanceGenerator()
                });

            emailService.SendEmail(emailAssembler.GenerateEmail());
            Log.Info($"Email Summary job finished, time {DateTime.Now.ToLocalTime()}.");
        }

        private IHtmlGenerator GetBinanceGenerator()
        {
            var symbolValues = binanceService.GetSymbolValuesForAccount();
            var binanceGenerator = emailGeneratorFactory.GetGenerator(EmailGenerator.Binance);
            binanceGenerator.SetBodyData(symbolValues);

            return binanceGenerator;
        }

        private IHtmlGenerator GetXtbGenerator()
        {
            var balance = xtbService.GetBalance();
            XtbOutput balanceOutput = mapper.Map<XtbOutput>(balance);
            var xtbHtmlGenerator = emailGeneratorFactory.GetGenerator(EmailGenerator.Xtb);
            xtbHtmlGenerator.SetBodyData(balanceOutput);

            return xtbHtmlGenerator;
        }

        private IHtmlGenerator GetOtodomGenerator()
        {
            var privateOffers = otoDomRepository.GetPrivateFlats();
            var mappedPrivate = MapToFlatsBM(privateOffers);
            var privateFlatAggregate = new FlatAggregateVM(mappedPrivate);

            var allOffers = otoDomRepository.GetActiveFlats();
            var flatDataBMs = MapToFlatsBM(allOffers);
            var allFlatAggregate = new FlatAggregateVM(flatDataBMs);
            SaveFlatSeries(allOffers);

            var flatsOutput = new FlatOutput(privateFlatAggregate.FlatCalculations, allFlatAggregate.FlatCalculations);
            var otodomHtmlGenerator = emailGeneratorFactory.GetGenerator(EmailGenerator.Otodom);
            otodomHtmlGenerator.SetBodyData(flatsOutput);

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

        private void SaveFlatSeries(IEnumerable<Flat> flats)
        {
            var flatSeries = new FlatSeries();
            flatSeries.Amount = flats.Count();
            flatSeries.AvgPrice = flats.Average(x => x.TotalPrice);
            flatSeries.AvgPricePerMeter = flatSeries.AvgPrice / flats.Average(x => x.Surface);

            flatSeries.BestValueId = flats.MinBy(x => x.TotalPrice / x.Surface).FirstOrDefault()?.Id;

            flatSeries.BiggestId = flats.MaxBy(x => x.Surface).FirstOrDefault()?.Id;
            flatSeries.MostExpensiveId = flats.MaxBy(x => x.TotalPrice).FirstOrDefault()?.Id;

            flatSeries.SmallestId = flats.MinBy(x => x.Surface).FirstOrDefault()?.Id;
            flatSeries.CheapestId = flats.MinBy(x => x.TotalPrice).FirstOrDefault()?.Id;

            flatSeries.DateFetched = DateTime.Now.Date;

            otoDomRepository.AddFlatSeries(flatSeries);
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
