using Data.EF.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repository.Interfaces
{
    public interface IOtoDomRepository
    {
        Flat GetFlatByExternalId(string otodomId);
        void SaveFlat(Flat flat);
        void SaveFlats(IEnumerable<Flat> flats);
    }
}
