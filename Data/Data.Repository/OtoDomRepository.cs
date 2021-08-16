using Data.EF.Models;
using Data.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Data.Repository
{
    public class OtoDomRepository : IOtoDomRepository
    {
        PiContext PiContext;
        private readonly Func<PiContext> contextMaker;

        public OtoDomRepository(Func<PiContext> contextMaker)
        {
            this.contextMaker = contextMaker;
            PiContext = contextMaker.Invoke();
        }

        public void Dispose()
        {
            PiContext.Dispose();
        }

        public Flat GetFlatByExternalId(string otodomId)
        {
            var flat = PiContext.Flat.FirstOrDefault(x => x.OtoDomId == otodomId);

            return flat;
        }

        public void AddFlats(IEnumerable<Flat> flats)
        {
            using (var context = contextMaker.Invoke())
            {
                context.Flat.AddRange(flats);
                context.SaveChanges();
            }
        }

        public void UpdateFlats(IEnumerable<Flat> flats)
        {
            using (var context = contextMaker.Invoke())
            {
                context.Flat.UpdateRange(flats);
                context.SaveChanges();
            }
        }

        public void DetachFlats(IEnumerable<Flat> flats)
        {
            foreach(var flat in flats)
            {
                PiContext.Entry(flat).State = EntityState.Detached;
            }
        }


        public IEnumerable<Flat> GetPrivateFlats()
        {
            var yesterdayDate = DateTime.Now.AddDays(-1);
            var privateFlats = PiContext.Flat
                .Where(x => 
                    (x.IsPrivate != null && x.IsPrivate.Value)
                    && (x.UpdatedDate != null && x.UpdatedDate > yesterdayDate)
                    && x.TotalPrice > 100000
                    && x.Surface > 1
                    && x.Surface < 500)
                .ToList();

            return privateFlats;
        }

        public IEnumerable<Flat> GetActiveFlats()
        {
            var yesterdayDate = DateTime.Now.AddDays(-1);

            var activeFlats = PiContext.Flat
                .Where(x => 
                    x.UpdatedDate != null 
                    && x.UpdatedDate > yesterdayDate 
                    && x.TotalPrice > 100000
                    && x.Surface > 1
                    && x.Surface < 500)
                .ToList();

            return activeFlats;
        }

        public void AddFlatSeries(FlatSeries flatSeries)
        {
            using (var context = contextMaker.Invoke())
            {
                context.FlatSeries.Add(flatSeries);
                context.SaveChanges();
            }
        }

        public IEnumerable<FlatSeries> GetFlatSeries()
        {
            using (var context = contextMaker.Invoke())
            {
                var flatSeries = context.FlatSeries
                    .Where(x => x.AvgPricePerMeter > 0)
                    .ToList();

                return flatSeries;
            }
        }
    }
}
