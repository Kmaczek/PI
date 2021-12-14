using System.Threading.Tasks;

namespace Data.Repository.Interfaces
{
    public interface ISettingRepository
    {
        /// <summary>
        /// Gets setting from database and converts to type.
        /// </summary>
        /// <typeparam name="T">The Type to convert to.</typeparam>
        /// <param name="name">Setting name.</param>
        /// <returns>Setting converted to type T</returns>
        T GetSetting<T>(string name);

        /// <summary>
        /// Gets setting asynchronously from database and converts it to type.
        /// </summary>
        /// <typeparam name="T">The Type to convert to.</typeparam>
        /// <param name="name">Setting name.</param>
        /// <returns>Setting converted to type T</returns>
        Task<T> GetSettingAsync<T>(string name);

        /// <summary>
        /// Gets setting from database and converts to Nullable type.
        /// </summary>
        /// <typeparam name="T">The Type to convert to.</typeparam>
        /// <param name="name">Setting name.</param>
        /// <returns>Setting converted to Nullable of T</returns>
        T? GetNullableSetting<T>(string name) where T : struct;

        /// <summary>
        /// Gets setting asynchronously from database and converts to Nullable type.
        /// </summary>
        /// <typeparam name="T">The Type to convert to.</typeparam>
        /// <param name="name">Setting name.</param>
        /// <returns>Setting converted to Nullable of T</returns>
        Task<T?> GetNullableSettingAsync<T>(string name) where T : struct;
    }
}