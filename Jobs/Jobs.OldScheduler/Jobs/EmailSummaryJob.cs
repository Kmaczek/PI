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
using MoreLinq;
using System.Linq;
using System.Threading;
using Xtb.Core;

namespace Jobs.OldScheduler.Jobs
{
    public class EmailSummaryJob : IJob
    {
        private static Timer RunningTimer = null;
        private IConfigurationRoot _configuration;
        private XtbInterface _xtbService;
        private readonly IBinanceService _binanceService;
        private readonly IEmailService _emailService;
        private readonly IOtoDomRepository _otoDomRepository;
        private readonly IEmailGeneratorFactory _emailGeneratorFactory;
        private readonly ILogger _log;
        private readonly IMapper _mapper;

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
            _configuration = configuration;
            _log = log;
            _mapper = mapper;
            _xtbService = xtbService;
            _binanceService = binanceService;
            _emailService = emailService;
            _otoDomRepository = otoDomRepository;
            _emailGeneratorFactory = emailGeneratorFactory;
        }

        public void Run()
        {
            EmailJob();
        }

        public void ImmediateRun()
        {
            _log.Info($"Immediate execution of {JobName}.");
            EmailJob();
            _log.Info($"Job {JobName} done.");
        }

        public void EmailJob()
        {
            _log.Info($"Processing Email Summary job, time {DateTime.Now.ToLocalTime()}.");

            try
            {
                var generators = new List<IHtmlGenerator>
                {
                    GetXtbGenerator(),
                    GetOtodomGenerator(),
                    GetBinanceGenerator()
                };
                EmailAssembler emailAssembler = new EmailAssembler(generators);

                _emailService.SendEmail(emailAssembler.GenerateEmail());
                _log.Info($"Email Summary job finished, time {DateTime.Now.ToLocalTime()}.");
            }
            catch(Exception e)
            {
                _log.Error($"Exception happened when trying to generate email.", e);
            }
        }

        private IHtmlGenerator GetBinanceGenerator()
        {
            var symbolValues = _binanceService.GetSymbolValuesForAccount();
            var binanceGenerator = _emailGeneratorFactory.GetGenerator(EmailGenerator.Binance);
            binanceGenerator.SetBodyData(symbolValues);

            return binanceGenerator;
        }

        private IHtmlGenerator GetXtbGenerator()
        {
            var balance = _xtbService.GetBalance();
            XtbOutput balanceOutput = _mapper.Map<XtbOutput>(balance);
            var xtbHtmlGenerator = _emailGeneratorFactory.GetGenerator(EmailGenerator.Xtb);
            xtbHtmlGenerator.SetBodyData(balanceOutput);

            return xtbHtmlGenerator;
        }

        private IHtmlGenerator GetOtodomGenerator()
        {
            var privateOffers = _otoDomRepository.GetPrivateFlats();
            var mappedPrivate = MapToFlatsBM(privateOffers);
            var privateFlatAggregate = new FlatAggregateVm(mappedPrivate);

            var allOffers = _otoDomRepository.GetActiveFlats();
            var flatDataBMs = MapToFlatsBM(allOffers);
            var allFlatAggregate = new FlatAggregateVm(flatDataBMs);
            SaveFlatSeries(allOffers);

            var flatsOutput = new FlatOutput(privateFlatAggregate.FlatCalculations, allFlatAggregate.FlatCalculations);
            var otodomHtmlGenerator = _emailGeneratorFactory.GetGenerator(EmailGenerator.Otodom);
            otodomHtmlGenerator.SetBodyData(flatsOutput);

            return otodomHtmlGenerator;
        }

        private List<FlatDataBm> MapToFlatsBM(IEnumerable<Data.EF.Models.Flat> allOffers)
        {
            var flatDataBMs = new List<FlatDataBm>();
            foreach (var flat in allOffers)
            {
                var flatBM = new FlatDataBm(
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

            _otoDomRepository.AddFlatSeries(flatSeries);
        }

        private void LogFlatScrappingErrors(IEnumerable<FlatDataBm> flats)
        {
            var flatsWithErrors = flats.Where(x => x.Errors.Any());
            if (flatsWithErrors.Any())
            {
                _log.Info($"Detected {flatsWithErrors.Count()} flats with errors. Printing them now.");

                foreach (var flat in flatsWithErrors)
                {
                    var joinedErrors = string.Join(Environment.NewLine, flat.Errors);
                    _log.Info($"Flat errors: {Environment.NewLine}{joinedErrors}.");
                }
            }
        }
    }
}
