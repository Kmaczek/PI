using Data.EF.Models;
using Data.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class SettingRepository: ISettingRepository
    {
        private readonly Func<PiContext> _contextMaker;

        public SettingRepository(Func<PiContext> contextMaker)
        {
            _contextMaker = contextMaker;
        }

        public T GetSetting<T>(string name)
        {
            return GetSettingAsync<T>(name).Result;
        }

        public async Task<T> GetSettingAsync<T>(string name)
        {
            using (var context = _contextMaker())
            {
                var settingValue = await context.Settings.FirstOrDefaultAsync(s => s.Name == name);

                if (settingValue == null) throw new Exception($"Setting {name} not found.");

                var convertedVal = Convert.ChangeType(name, typeof(T));

                return (T)convertedVal;
            }
        }

        public T? GetNullableSetting<T>(string name) where T: struct
        {
            return GetNullableSettingAsync<T>(name).Result;
        }

        public async Task<T?> GetNullableSettingAsync<T>(string name) where T : struct
        {
            using (var context = _contextMaker())
            {
                var setting = await context.Settings.FirstOrDefaultAsync(s => s.Name == name);

                if (setting == null || setting.Value == null || string.IsNullOrEmpty(setting.Value)) return null;

                var convertedVal = Convert.ChangeType(name, typeof(T));

                return (T)convertedVal;
            }
        }
    }
}
