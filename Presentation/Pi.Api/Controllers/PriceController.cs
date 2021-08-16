using Core.Domain.Logic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pi.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PriceController : ControllerBase
    {
        private readonly ILogger<FlatController> _logger;
        private readonly IPriceService _priceService;

        public PriceController(
            ILogger<FlatController> logger,
            IPriceService priceService)
        {
            _logger = logger;
            _priceService = priceService;
        }
    }
}
