﻿using Core.Model.PriceView;
using Data.EF.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repository.Interfaces
{
    public interface IPriceRepository
    {
        void SavePriceDetails(PriceDetails priceDetails, int ParserId);
        void SavePrices(IEnumerable<PriceSeries> priceSeries);

        IEnumerable<Parser> GetParsers();
        IEnumerable<Parser> GetParsers(IEnumerable<int> parsersToRun);
        IEnumerable<PriceDetails> GetPriceDetails();
        IEnumerable<GrouppedProductsVm> GetProductsGrouppedBySite();
        IEnumerable<PriceSeries> GetMaxPricesDetails();
        IEnumerable<PriceSeries> GetMinPricesDetails();
        IEnumerable<PriceSeries> GetPriceSeries(DateTime date);
        IEnumerable<PriceSeries> GetPriceSeries(int productId);
    }
}
