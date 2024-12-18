using Core.Model.PriceView;
using Data.EF.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces
{
    public interface IPriceRepository
    {
        void SavePriceDetails(PriceDetails priceDetails, int ParserId);
        void SavePrices(IEnumerable<PriceSeries> priceSeries);

        IEnumerable<Product> GetProductsActive();
        Task<Product> GetProduct(int productId);
        Task<IEnumerable<Product>> GetProducts(IEnumerable<int> parsersToRun);
        IEnumerable<PriceDetails> GetPriceDetails();
        Task<IEnumerable<GrouppedProductsVm>> GetProductsGrouppedBySite();
        IEnumerable<PriceSeries> GetMaxPricesDetails();
        IEnumerable<PriceSeries> GetMinPricesDetails();
        IEnumerable<PriceSeries> GetPriceSeries(DateTime date);
        Task<IEnumerable<PriceSeries>> GetPriceSeries(int productId);

        /// <summary>
        /// Gets all the price series for the given date range
        /// </summary>
        /// <param name="from">Inclusive start date</param>
        /// <param name="to">Exclusive end date</param>
        /// <returns></returns>
        Task<IEnumerable<PriceSeries>> GetPriceSeries(DateTime from, DateTime to);

        Task<bool> HealthCheck();
    }
}
