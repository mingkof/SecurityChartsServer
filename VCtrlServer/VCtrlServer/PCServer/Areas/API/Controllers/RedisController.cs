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
    [Route("api/redistest")]
    public class RedisController : Controller
    {
        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            return ConnectionMultiplexer.Connect("127.0.0.1:6379,abortConnect=false");
        });

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }

        private readonly ILogger _logger;

        private readonly IDatabase RedisDb;

        private static string Table = "Test";

        public RedisController(ILogger<RedisController> logger)
        {
            _logger = logger;
            RedisDb = Connection.GetDatabase();
            //if (RedisDb.IsConnected(Table) && (!RedisDb.KeyExists(Table) || !RedisDb.KeyType(Table).Equals(RedisType.List)))
            //{
            //    RedisDb.KeyDelete(Table);
            //    RedisDb.ListRightPush(Table, "A");
            //    RedisDb.ListRightPush(Table, "B");
            //    RedisDb.ListRightPush(Table, "C");
            //    RedisDb.ListRightPush(Table, "D");
            //}
        }

        [HttpGet("", Name = "redislist")]
        public IActionResult Get()
        {
            List<string> list = new List<string>();
            if (RedisDb.IsConnected(Table))
            {
                long length = RedisDb.ListLength(Table);
                list = RedisDb.ListRange(Table, 0, length).Select(o => (string)o).ToList();
            }
            return Ok(list);
        }
        [HttpPost]
        public IActionResult Post([FromBody]string value)
        {
            if (value == null || !ModelState.IsValid)
                return BadRequest();

            RedisDb.ListRightPush(Table, value);

            return Ok(value);
        }
        [HttpDelete("{value}")]
        public IActionResult Delete(string value)
        {
            if (RedisDb.IsConnected(Table))
            {
                RedisDb.ListRemove(Table, value);
            }
            return Ok(value);
        }
    }
}
