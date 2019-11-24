using AutoMapper;
using Core.Common;
using Core.Domain.Logic.OtodomService;
using Core.Model.Exceptions;
using Core.Model.FlatsModels;
using Data.EF.Models;
using Data.Repository.Interfaces;
using Flats.Core.Scraping;
using Microsoft.Extensions.Configuration;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Domain.Logic.FlatsFeed
{
    public class OtodomFeedService : IFlatsFeedService
    {
        private readonly ConfigHelper configuration;
        private readonly IOtoDomRepository otoDomRepository;
        private readonly IMapper mapper;
        private readonly ILogger log;
        private readonly IScrapper otoDomScrapper;

        private string otodomUrl = string.Empty;
        private string urlSection = string.Empty;
        private string serviceName = nameof(OtodomFeedService);
        private Dictionary<string, int> usedKeys = new Dictionary<string, int>();

        public OtodomFeedService(
            ConfigHelper configHelper,
            ILogger log,
            IScrapper otoDomScrapper,
            IOtoDomRepository otoDomRepository,
            IMapper mapper)
        {
            this.log = log;
            this.otoDomScrapper = otoDomScrapper;
            this.otoDomRepository = otoDomRepository;
            this.mapper = mapper;

            if (configHelper == null || configHelper.OtodomFeedJobConfig == null)
                throw new OtodomServiceException($"Missing configuration provider for {serviceName}.");

            otodomUrl = configHelper.OtodomFeedJobConfig.AllOffersUrl;

            if (string.IsNullOrEmpty(otodomUrl))
                throw new OtodomServiceException($"Missing configuration section [OtodomFeedJob:otodomUrlAllOffers] for {serviceName}.");
        }

        public FeedStats FeedStats { get; } = new FeedStats(nameof(OtodomFeedService));

        public void StartFeedProcess(string url = null)
        {
            if (string.IsNullOrEmpty(url))
            {
                otoDomScrapper.ScrapingUrl = otodomUrl;
            }
            else
            {
                otoDomScrapper.ScrapingUrl = url;
            }
            
            otoDomScrapper.OnScrapedPage += PersistScrappedFlats;

            var scrappedFlats = otoDomScrapper.Scrape();

            otoDomScrapper.OnScrapedPage -= PersistScrappedFlats;
            UpdateStatsWithErrors();

            otoDomRepository.AddFlatSeries(new FlatSeries());
        }

        public void GenerateFlatSeries()
        {
            var activeFlats = otoDomRepository.GetActiveFlats();

        }

        private void PersistScrappedFlats(IEnumerable<FlatDataBM> scrappedFlats)
        {
            var flatsToUpdate = new List<Flat>();
            var flatsToAdd = new List<Flat>();

            try
            {
                var distinctScrappedFlats = scrappedFlats
                    .DistinctBy(x => x.OtoDomId)
                    .Where(x => !usedKeys.ContainsKey(x.OtoDomId));
                var flats = mapper.Map<IEnumerable<FlatDataBM>, IEnumerable<Flat>>(distinctScrappedFlats);

                foreach (var flat in flats)
                {
                    if (usedKeys.ContainsKey(flat.OtoDomId))
                        usedKeys[flat.OtoDomId]++;
                    else
                        usedKeys[flat.OtoDomId] = 1;

                    var dbFlat = otoDomRepository.GetFlatByExternalId(flat.OtoDomId);
                    if(dbFlat != null)
                    {
                        flat.Id = dbFlat.Id;
                        flat.CreatedDate = dbFlat.CreatedDate;
                    }
                    else
                    {
                        flat.CreatedDate = DateTime.Now;
                    }

                    if(!flat.CreatedDate.HasValue)
                        flat.CreatedDate = DateTime.Now;

                    flat.UpdatedDate = DateTime.Now;
                }

                flatsToUpdate.Clear();
                flatsToUpdate.AddRange(flats.Where(x => x.Id != 0).ToList());
                log.Info($"Flats to update {flatsToUpdate.Count()}");

                flatsToAdd.Clear();
                flatsToAdd.AddRange(flats.Where(x => x.Id == 0).ToList());
                log.Info($"Flats to add {flatsToAdd.Count()}");

                UpdateFlats(flatsToUpdate);
                AddFlats(flatsToAdd);

                UpdateStats(flatsToUpdate, flatsToAdd);
            }
            catch (Exception e)
            {
                otoDomRepository.DetachFlats(flatsToUpdate);
                otoDomRepository.DetachFlats(flatsToAdd);

                log.Error("Error during saving entities to DB.", e);
            }
        }

        private IEnumerable<Flat> CheckIfEntitiesAttached(List<Flat> flatsToUpdate)
        {
            return otoDomRepository.CheckIfEntitiesAttached(flatsToUpdate);
        }

        private static void SetFlatsToUpdate(IEnumerable<Flat> flats, List<Flat> flatsToUpdate)
        {
            flatsToUpdate.Clear();
            flatsToUpdate.AddRange(flats.Where(x => x.Id != 0).ToList());
        }

        private void UpdateStats(IEnumerable<Flat> flatsToUpdate, IEnumerable<Flat> flatsToAdd)
        {
            log.Info($"Flats before save U:{flatsToUpdate.Count()} A: {flatsToAdd.Count()}");
            FeedStats.Updated += flatsToUpdate.Count();
            FeedStats.Added += flatsToAdd.Count();

            log.Info($"Flats after save U:{FeedStats.Updated} A: {FeedStats.Added}");
        }

        private void UpdateStatsWithErrors()
        {
            FeedStats.Errors.Clear();
            FeedStats.Errors.AddRange(otoDomScrapper.Errors);
        }

        private void AddFlats(IEnumerable<Flat> flatsToAdd)
        {
            if (flatsToAdd.Any())
            {
                otoDomRepository.AddFlats(flatsToAdd);
            }
        }

        private void UpdateFlats(IEnumerable<Flat> flatsToUpdate)
        {
            if (flatsToUpdate.Any())
            {
                otoDomRepository.UpdateFlats(flatsToUpdate);
            }
        }
    }
}
