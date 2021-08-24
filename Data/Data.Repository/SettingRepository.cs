using Data.EF.Models;
using System;
using System.Linq;

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
            using (var context = _contextMaker())
            {
                var settingValue = context.Settings.FirstOrDefault(s => s.Name == name);

                if (settingValue == null) throw new Exception($"Setting {name} not found.");

                var convertedVal = Convert.ChangeType(name, typeof(T));

                return (T)convertedVal;
            }
        }
    }
}
