using Data.EF.Dto;
using Data.EF.Models;
using Data.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public IEnumerable<FlatId> GetAllExternalIds()
        {
            var flatIds = PiContext.Flat.Select(x => new FlatId { Id = x.Id, OtoDomId = x.OtoDomId });

            return flatIds;
        }

        public void SaveFlat(Flat flat)
        {
            using (var context = contextMaker.Invoke())
            {
                context.Flat.Add(flat);
            }
        }

        public int GetFlatIdByExternalId(string externalId)
        {
            var id = PiContext.Flat
                .Where(x => x.OtoDomId == externalId)
                .Select(x => x.Id)
                .FirstOrDefault();

            return id;
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

        public IEnumerable<Flat> CheckIfEntitiesAttached(List<Flat> flats)
        {
            var attachedEtities = new List<Flat>();
            foreach (var flat in flats)
            {
                var attachedEntity =  PiContext.Flat.Local.FirstOrDefault(e => e == flat);
                if (attachedEntity != null)
                {
                    attachedEtities.Add(attachedEntity);
                }
            }

            return attachedEtities;
        }

        public void SaveContext()
        {
            PiContext.SaveChanges();
        }

        public IEnumerable<Flat> GetPrivateFlats()
        {
            var yesterdayDate = DateTime.Now.AddDays(-1);
            var privateFlats = PiContext.Flat
                .Where(x => 
                    (x.IsPrivate != null && x.IsPrivate.Value)
                    && (x.UpdatedDate != null && x.UpdatedDate > yesterdayDate))
                .ToList();

            return privateFlats;
        }

        public IEnumerable<Flat> GetActiveFlats()
        {
            var yesterdayDate = DateTime.Now.AddDays(-1);

            var activeFlats = PiContext.Flat
                .Where(x => x.UpdatedDate != null && x.UpdatedDate > yesterdayDate && x.TotalPrice > 1)
                .ToList();

            return activeFlats;
        }
    }
}
