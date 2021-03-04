using Data.EF.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repository.Interfaces
{
    public interface IOtoDomRepository
    {
        void AddFlats(IEnumerable<Flat> flats);
        void UpdateFlats(IEnumerable<Flat> flats);
        void DetachFlats(IEnumerable<Flat> flats);
        void AddFlatSeries(FlatSeries flatSeries);

        Flat GetFlatByExternalId(string otodomId);
        IEnumerable<Flat> GetPrivateFlats();
        IEnumerable<Flat> GetActiveFlats();
        IEnumerable<FlatSeries> GetFlatSeries();
    }
}
