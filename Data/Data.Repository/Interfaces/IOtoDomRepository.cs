using Data.EF.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repository.Interfaces
{
    public interface IOtoDomRepository
    {
        int GetFlatIdByExternalId(string externalId);
        Flat GetFlatByExternalId(string otodomId);
        void SaveFlat(Flat flat);
        void AddFlats(IEnumerable<Flat> flats);
        void UpdateFlats(IEnumerable<Flat> flats);
        IEnumerable<Flat> CheckIfEntitiesAttached(List<Flat> flats);
        void DetachFlats(IEnumerable<Flat> flats);
        void SaveContext();
        IEnumerable<Flat> GetPrivateFlats();
        IEnumerable<Flat> GetActiveFlats();
    }
}
