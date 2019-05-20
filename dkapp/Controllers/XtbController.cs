using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Xtb.Core;

namespace dkapp.Controllers
{
    [Produces("application/json")]
    [Route("api/Xtb")]
    public class XtbController : Controller
    {
        public XtbController()
        {
            //inject xtb here
        }

        // GET: api/Xtb
        [HttpGet]
        public string Get()
        {
            //var xtbService = new Service();
            //var srvTime = xtbService.GetServerTime();
            //return srvTime.TimeString;

            return DateTime.Now.ToShortDateString();
        }

        // GET: api/Xtb/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }
        
        // POST: api/Xtb
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/Xtb/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        
    }
}
