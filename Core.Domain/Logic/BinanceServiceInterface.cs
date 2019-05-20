using Binance.Api.BinanceDto;
using Core.Model.BinanceModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Domain.Logic
{
    public interface BinanceServiceInterface
    {
        BinanceVM GetSymbolValuesForAccount();
    }
}
