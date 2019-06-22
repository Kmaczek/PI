using Data.EF.Dto;
using Data.EF.Models;
using Data.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repository
{
    public class OtoDomRepository : IOtoDomRepository
    {
        PiContext PiContext;

        public OtoDomRepository(PiContext dbContext)
        {
            PiContext = dbContext;
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
            PiContext.Flat.Add(flat);
        }

        public async void SaveFlats(IEnumerable<Flat> flats)
        {
            await PiContext.Flat.AddRangeAsync(flats);
        }

        public async void UpdateFlats(IEnumerable<Flat> flats)
        {
            await PiContext.Flat.AddRangeAsync(flats);
        }
    }
}
