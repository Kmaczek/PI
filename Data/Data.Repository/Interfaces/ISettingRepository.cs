namespace Data.Repository.Interfaces
{
    public interface ISettingRepository
    {
        T GetSetting<T>(string name);
    }
}