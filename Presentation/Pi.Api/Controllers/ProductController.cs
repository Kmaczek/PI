using Core.Domain.Logic;
using Core.Model.PriceView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pi.Api.Controllers
{
    [ApiController]
    [Route("product")]
    public class ProductController : SecureController
    {
        private readonly ILogger<FlatController> _logger;
        private readonly IPriceService _priceService;

        public ProductController(
            ILogger<FlatController> logger,
            IPriceService priceService)
        {
            _logger = logger;
            _priceService = priceService;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ProductVm> GetSingle(int id)
        {
            return await _priceService.GetProductDetails(id);
        }

        [HttpGet]
        public async Task<IEnumerable<GrouppedProductsVm>> Get()
        {
            var products = await _priceService.GetProducts();

            return products;
        }

        [HttpGet("series")]
        public async Task<IEnumerable<PriceSeriesVm>> GetPriceSeries(int priceDetailsId)
        {
            var products = await _priceService.GetPriceSeries(priceDetailsId);

            return products;
        }
    }
}
