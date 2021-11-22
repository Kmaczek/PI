using Data.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Domain.Logic._2Miners
{
    public class TwoMinersService : ITwoMinersService
    {
        private readonly ISettingRepository _settingRepository;

        public TwoMinersService(ISettingRepository settingRepository)
        {
            this._settingRepository = settingRepository;
        }

        public void AccountInfo(string walletId)
        {
            throw new NotImplementedException();
        }
    }
}
