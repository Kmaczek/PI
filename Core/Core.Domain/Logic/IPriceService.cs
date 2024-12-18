using Core.Model.PriceView;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Domain.Logic
{
    public interface IPriceService
    {
        Task<IEnumerable<GrouppedProductsVm>> GetProducts();
        Task<ProductVm> GetProductDetails(int productId);
        Task<IEnumerable<PriceSeriesVm>> GetPriceSeries(int productId);
    }
}
