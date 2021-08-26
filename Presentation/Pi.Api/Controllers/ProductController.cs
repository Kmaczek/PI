using Core.Domain.Logic;
using Core.Model.PriceView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Pi.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
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

        [HttpGet]
        public IEnumerable<GrouppedProductsVm> Get()
        {
            var products = _priceService.GetProducts();

            return products;
        }

        [HttpGet("series")]
        public IEnumerable<PriceSeriesVm> GetPriceSeries(int productId)
        {
            var products = _priceService.GetPriceSeries(productId);

            return products;
        }
    }
}
