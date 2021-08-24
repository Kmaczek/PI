namespace Data.Repository
{
    public interface ISettingRepository
    {
        T GetSetting<T>(string name);
    }
}