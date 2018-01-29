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
    [Route("api/traviodata")]
    public class TravioDataController : Controller
    {
        private readonly ILogger _logger;
        private readonly ITravioDataRepositoy _traviodata;
        public TravioDataController(ITravioDataRepositoy traviodata,ILogger<Sys110WarnController> logger)
        {
            _logger = logger;
            _traviodata = traviodata;
        }
        /// <summary>
        /// 获取某年某月某日 抓拍 数量
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        [HttpGet("GetDayCount/{year}/{month}/{day}")]
        public IActionResult GetDayCount(string year,string month,string day)
        {
            var query=_traviodata.Find(p=>p.Year==year&&p.Month==month&&p.Day==day);
            if (query!=null)
            {
                return Ok(new {
                    res=query.TodayCount
                });
            }
            else
            {
                return BadRequest("数据为空");
            }

        }
    }
 }