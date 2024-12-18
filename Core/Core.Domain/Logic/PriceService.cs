using Core.Model.PriceView;
using Data.Repository.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Domain.Logic
{
    public class PriceService(IPriceRepository priceRepository) : IPriceService
    {
        public Task<IEnumerable<GrouppedProductsVm>> GetProducts()
        {
            return priceRepository.GetProductsGrouppedBySite();
        }

        public async Task<IEnumerable<PriceSeriesVm>> GetPriceSeries(int priceDetailsId)
        {
            var priceSeries = await priceRepository.GetPriceSeries(priceDetailsId);
            var series = new List<PriceSeriesVm>();
            foreach (var ps in priceSeries)
            {
                var serie = new PriceSeriesVm();
                serie.Price = ps.Price;
                serie.CreatedDate = ps.CreatedDate;

                series.Add(serie);
            }

            return series;
        }

        public async Task<ProductVm> GetProductDetails(int productId)
        {
            return ProductVm.FromDm(await priceRepository.GetProduct(productId));
        }
    }
}
