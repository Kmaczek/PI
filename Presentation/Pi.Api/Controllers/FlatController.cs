using Core.Domain.Logic;
using Core.Model.FlatSerie;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Pi.Api.Controllers
{
    [ApiController]
    [Route("flat")]
    public class FlatController: SecureController
    {
        private readonly ILogger<FlatController> _logger;
        private readonly IFlatSeriesService flatSeriesService;

        public FlatController(
            ILogger<FlatController> logger,
            IFlatSeriesService flatSeriesService)
        {
            _logger = logger;
            this.flatSeriesService = flatSeriesService;
        }

        [HttpGet("series")]
        public IEnumerable<FlatSerieVm> Get()
        {
            var flatSerieVm = flatSeriesService.GetFlatSeries();

            return flatSerieVm;
        }
    }
}
