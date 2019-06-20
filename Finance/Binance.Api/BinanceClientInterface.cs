using Binance.Api.BinanceDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Binance.Api
{
    public interface BinanceClientInterface
    {
        bool Ping();
        ServerTimeDto ServerTime();
        ExchangeInfoDto ExchangeInfo();
        ExchangeInfoDto OrderBook();
        TradeDto HistoricalTrades();
        AveragePriceDto GetAveragePrice(string symbol);
        BinanceResponse<AccountInformationDto> AccountInfo();
    }
}
