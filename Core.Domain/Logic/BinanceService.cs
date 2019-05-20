using Binance.Api;
using Binance.Api.BinanceDto;
using Core.Common;
using Core.Model.BinanceModels;
using System;
using System.Linq;

namespace Core.Domain.Logic
{
    public class BinanceService : BinanceServiceInterface
    {
        private readonly BinanceClientInterface _binanceClient;
        private readonly LoggerInterface _log;

        public BinanceService(BinanceClientInterface binanceClient, LoggerInterface log)
        {
            this._binanceClient = binanceClient;
            this._log = log;
        }

        public BinanceVM GetSymbolValuesForAccount()
        {
            try
            {
                var binanceVM = new BinanceVM();

                var accountInfo = _binanceClient.AccountInfo();
                if (accountInfo.Status == OutcomeStatus.Failed)
                {
                    foreach (var notification in accountInfo.Notifications)
                        _log.Error($"[Severity: {notification.Severity}] Message: {notification.Message}");

                    binanceVM.Status = BinanceStatus.Error;
                    return binanceVM;
                }

                var info = accountInfo.ResponseDto;
                info.Balances = info.Balances.Where(x => x.Free > 0);

                foreach (var balance in info.Balances)
                {
                    var avgPrice = _binanceClient.GetAveragePrice(balance.Asset + "USDT");
                    binanceVM.SymbolsValues.Add(new BinanceSymbolValueVM()
                    {
                        Symbol = balance.Asset,
                        Amount = balance.Free,
                        AvgUsdPrice = avgPrice.Price
                    });
                }

                binanceVM.SymbolsValues = binanceVM.SymbolsValues.OrderByDescending(x => x.ConvertedPrice).ToList();
                binanceVM.Status = BinanceStatus.OK;

                return binanceVM;
            }
            catch(Exception e)
            {
                _log.Error("Unknown error occured while fetching Binance data", e);
                return BinanceVM.Empty;
            }
        }
    }
}
