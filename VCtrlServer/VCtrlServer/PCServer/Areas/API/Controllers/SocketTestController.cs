using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SHSecurityContext.IRepositorys;
using SHSecurityModels;
using System.Text;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PCServer.Controllers
{
    [Produces("application/json")]
    [Route("api/socket")]
    public class SocketTestController : Controller
    {
        private readonly ILogger _logger;

        public SocketTestController(ILogger<SocketTestController> logger)
        {
            _logger = logger;
        }

        [HttpGet("")]
        public IActionResult Get()
        {


            return Ok("OK");
        }
        //[HttpPost]
        //public IActionResult Post([FromBody]string value)
        //{
        //    if (value == null || !ModelState.IsValid)
        //        return BadRequest();

        //    RedisDb.ListRightPush(Table, value);

        //    return Ok(value);
        //}
        //[HttpDelete("{value}")]
        //public IActionResult Delete(string value)
        //{
        //    if (RedisDb.IsConnected(Table))
        //    {
        //        RedisDb.ListRemove(Table, value);
        //    }
        //    return Ok(value);
        //}
    }
}
