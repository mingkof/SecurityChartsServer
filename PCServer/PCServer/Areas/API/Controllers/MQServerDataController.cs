using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PCServer;
using SHSecurityContext.IRepositorys;
using SHSecurityModels;
using SHSecurityServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SHSecurityServer.Controllers
{
    [Produces("application/json")]
    [Route("api/MQServerData")]
    public class MQServerDataController : Controller
    {
        private readonly ILogger _logger;
        private readonly IMQServerDataRepository _mqServerData;
        public MQServerDataController(IMQServerDataRepository mqServerData, ILogger<Sys110WarnController> logger)
        {
            _logger = logger;
            _mqServerData = mqServerData;
        }

        [HttpGet("GetDataTimeRangeList/{startTime}/{endTime}")]
        public IActionResult GetDataTimeRangeList(int startTime,int endTime)
        {
            var query = _mqServerData.FindList(p => p.timeStamp >= startTime && p.timeStamp <= endTime, "", false);

            return Ok(new {
                res=query
            });
        }
      
    }
 }