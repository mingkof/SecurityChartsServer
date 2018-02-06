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
using KVDDDCore.Utils;

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

        /// <summary>
        /// 设置摄像头数人的数量
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        [HttpPost("AddRange")]
        public IActionResult AddRange([FromBody] List<camPeopleCount> values)
        {
            if (values == null)
                return BadRequest();

            for (int i = 0; i < values.Count; i++)
            {
                var item = values[i];

                if (string.IsNullOrEmpty(item.ID))
                    continue;
                var dateTime = TimeUtils.ConvertToDateTime(item.Time);
                var year = dateTime.Year.ToString();
                var month = dateTime.Month.ToString("00");
                var day = dateTime.Day.ToString("00");
                var hour = dateTime.Hour.ToString("00");
                var minute = dateTime.Minute.ToString("00");

                var query = _camPeopleCount.Find(p => p.ID == item.ID&&p.Year==year&&p.Month==month&&p.Day==day&&p.Hour==hour&&p.Minute==minute);
                if (query == null)
                {
                    _camPeopleCount.Add(new sys_camPeopleCount {
                        ID=item.ID,
                        Count=item.Count,
                        Time=item.Time,
                        Year=year,
                        Month=month,
                        Day=day,
                        Hour=hour,
                        Minute=minute
                    });
                }
                else
                {
                    query.Count = item.Count;
                    query.Time = item.Time;
                    query.Year = year;
                    query.Month = month;
                    query.Day = day;
                    query.Hour = hour;
                    query.Minute = minute;
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
public class camPeopleCount
{
    public string ID { get; set; }
    public int Count { get; set; }
    public int Time { get; set; }
}
