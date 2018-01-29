using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PCServer;
using SHSecurityContext.IRepositorys;
using SHSecurityModels;
using SHSecurityServer.Models;
using System;
using System.Collections.Generic;
using KVDDDCore.Utils;
namespace SHSecurityServer.Controllers
{
    [Produces("application/json")]
    [Route("api/traviodata")]
    public class HongWaiDataController : Controller
    {
        private readonly ILogger _logger;
        private readonly IHongWaiPeopleDataRepositoy _hongwaidata;
        public HongWaiDataController(IHongWaiPeopleDataRepositoy hongwaidata, ILogger<Sys110WarnController> logger)
        {
            _logger = logger;
            _hongwaidata = hongwaidata;
        }
        /// <summary>
        /// 根据设备编号获取其进出信息
        /// </summary>
        /// <param name="sn"></param>
        /// <returns></returns>
        [HttpGet("GetSnCountData/{sn}")]
        public IActionResult GetSnCountData(string sn)
        {
            string today = DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00";
            int todayStamp = TimeUtils.ConvertToTimeStamps(today);
            //string nowYear = System.DateTime.Now.Year.ToString();
            //string nowMonth = System.DateTime.Now.Month.ToString("00");
            //string nowDay = System.DateTime.Now.Day.ToString("00");
            var query = _hongwaidata.FindList(p => p.sn == sn && p.timeStamp >= todayStamp,"",false);
            return Ok(new {
                res = query
            });
        }
    }
 }