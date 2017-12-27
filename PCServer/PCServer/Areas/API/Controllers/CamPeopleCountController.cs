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
    [Route("api/camPeopleCount")]
    public class CamPeopleCountController : Controller
    {
        private readonly ILogger _logger;
        private readonly ICamePeopleCountRepository _camPeopleCount;
        public CamPeopleCountController(ICamePeopleCountRepository camPeopleCount, ILogger<CamPeopleCountController> logger)
        {
            _logger = logger;
            _camPeopleCount = camPeopleCount;
        }

        [HttpGet("list")]
        public IActionResult GetList()
        {
            var list = _camPeopleCount.FindList(p => true,"",false);

            return Ok(new
            {
                res = list
            });
        }

        [HttpGet("get")]
        public IActionResult GetSingle(string id)
        {
            var query = _camPeopleCount.Find(p => p.ID == id);

            return Ok(new
            {
                res = query
            });
        }


        [HttpPost("AddRange")]
        public IActionResult AddRange([FromBody] List<sys_camPeopleCount> values)
        {
            if (values == null)
                return BadRequest();

            for (int i = 0; i < values.Count; i++)
            {
                var item = values[i];

                if (string.IsNullOrEmpty(item.ID))
                    continue;

                var query = _camPeopleCount.Find(p => p.ID == item.ID);
                if(query == null)
                {
                    _camPeopleCount.Add(item);
                }
                else
                {
                    query.Count = item.Count;
                    _camPeopleCount.Update(query);
                }
            }

            return Ok();
        }
    }
}
