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
using PCServer;
using System.Net.Http;
using System.Text;

namespace SHSecurityServer.Controllers
{
    [Produces("application/json")]
    [Route("api/camPeopleCount")]
    public class CamPeopleCountController : Controller
    {
        private readonly ILogger _logger;
        private readonly ICamePeopleCountRepository _camPeopleCount;
        private readonly IPeopleCountConfigRepositoy _repo_pconfig;

        public CamPeopleCountController(ICamePeopleCountRepository camPeopleCount,
             IPeopleCountConfigRepositoy repo_pconfig,ILogger<CamPeopleCountController> logger)
        {
            _logger = logger;
            _camPeopleCount = camPeopleCount;
            _repo_pconfig = repo_pconfig;
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
                if (query == null)
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


        /// <summary>
        /// 设置人数统计配置
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("SetPeopleCountConfig")]
        public IActionResult SetPeopleCountConfig([FromBody]sys_PeopleCountConfig value)
        {
            if(value == null || string.IsNullOrEmpty(value.Id))
            {
                return BadRequest();
            }

            var query = _repo_pconfig.Find(p => p.Id == value.Id);

            if(query == null)
            {
                _repo_pconfig.Add(value);
            } else
            {
                query.Content = value.Content ?? "";
                _repo_pconfig.Update(query);

            }

            return Ok();
        }

        /// <summary>
        /// 设置/更新人数统计配置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetPeopleCountConfig/{id}")]
        public IActionResult GetPeopleCountConfig(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            var query = _repo_pconfig.Find(p => p.Id == id);
            //var query = _repo_pconfig.FromSql($"select Content from peoplecountconfig WHERE Id={id}").FirstOrDefault();
            if (query == null)
                return NotFound();

            return Content(query.Content);

            //HttpResponseMessage result = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            //result.Content = new StringContent(query.Content, Encoding.GetEncoding("UTF-8"), "application/json");
            //return result;
        }

    }
    }
