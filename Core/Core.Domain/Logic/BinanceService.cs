using Binance.Api;
using Binance.Api.BinanceDto;
using Core.Common;
using Core.Model.BinanceModels;
using Data.EF.Models;
using Data.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Domain.Logic
{
    public class BinanceService : IBinanceService
    {
        private readonly IBinanceClient _binanceClient;
        private readonly ILogger _log;
        private readonly IBinanceRepository binanceRepository;

        public BinanceService(IBinanceClient binanceClient, ILogger log, IBinanceRepository binanceRepository)
        {
            this._binanceClient = binanceClient;
            this._log = log;
            this.binanceRepository = binanceRepository;
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

                //TODO: Split this and add saving of series in separate method and separate job
                AddSeries(binanceVM);

                binanceVM.SymbolsValues = binanceVM.SymbolsValues.OrderByDescending(x => x.ConvertedPrice).ToList();
                binanceVM.Status = BinanceStatus.OK;

                return binanceVM;
            }
            catch (Exception e)
            {
                _log.Error("Unknown error occured while fetching Binance data", e);
                return BinanceVM.Empty;
            }
        }

        private void AddSeries(BinanceVM binanceVM)
        {
            var seriesList = new List<Series>();
            seriesList.AddRange(binanceVM.SymbolsValues.Select(x => new Series()
            {
                Ammount = x.Amount,
                AvgPrice = x.AvgUsdPrice,
                Currency = x.Symbol
            }));

            binanceRepository.SaveSeriesParent(new SeriesParent()
            {
                FetchedDate = DateTime.Now,
                Total = binanceVM.TotalValue,
                Series = seriesList
            });
        }
    }
}
