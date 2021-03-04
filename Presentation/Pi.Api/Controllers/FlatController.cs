using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domain.Logic;
using Core.Model.FlatSerie;
using Data.EF.Models;
using Data.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Pi.Api.Controllers
{
    [ApiController]
    //[Authorize]
    [Route("[controller]")]
    public class FlatController : ControllerBase
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
