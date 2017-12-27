using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SHSecurityContext.IRepositorys;
using SHSecurityModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.NodeServices;
namespace SHSecurityServer.Controllers
{
    [Produces("application/json")]
    [Route("api/policegps")]
    public class PoliceGpsController : Controller
    {
        private readonly ILogger _logger;
        private readonly IPoliceGpsRepository _policeGps;
        public PoliceGpsController(IPoliceGpsRepository policeGps, ILogger<PoliceGpsController> logger)
        {
            _logger = logger;
            _policeGps = policeGps;
        }

        [HttpGet("list")]
        public IActionResult GetList()
        {
            var list = _policeGps.FindList(p => true,"",false);

            return Ok(new
            {
                PoliceArray = list
            });
        }
    }
}
