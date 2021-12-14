using Data.Repository.Interfaces;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Core.Domain.Logic._2Miners
{
    public class TwoMinersService : ITwoMinersService
    {
        private readonly ISettingRepository _settingRepository;
        private readonly IHttpClientFactory _httpClientFactory;

        public TwoMinersService(
            ISettingRepository settingRepository,
            IHttpClientFactory httpClientFactory)
        {
            this._settingRepository = settingRepository;
            this._httpClientFactory = httpClientFactory;
        }

        public string ApiUrl { get; private set; }
        public string Wallet { get; private set; }

        public async Task LoadApi(string api)
        {
            ApiUrl = await _settingRepository.GetSettingAsync<string>($"2Miners{api}");
            Wallet = await _settingRepository.GetSettingAsync<string>($"2Miners{api}");
        }

        public async Task AccountInfoAsync(string walletId)
        {
            var client = _httpClientFactory.CreateClient();
        }
    }

    public class TwoMinersApi
    {
        public const string ETH = "Eth";
        public const string RVN = "Rvn";
    }
}
