using System.Collections.Generic;
using Core.Domain.Logic;
using Core.Model.FlatSerie;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Pi.Api.Controllers
{
    [ApiController]
    //[Authorize]
    [Route("[controller]")]
    public class FlatController
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
