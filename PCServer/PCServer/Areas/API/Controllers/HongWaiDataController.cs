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
        private readonly IHongWaiPeopleHistoryDataRepositoy _hongwaiHistorydata;

        public HongWaiDataController(IHongWaiPeopleDataRepositoy hongwaidata, IHongWaiPeopleHistoryDataRepositoy hongwaiHistorydata, ILogger<Sys110WarnController> logger)
        {
            _logger = logger;
            _hongwaidata = hongwaidata;
            _hongwaiHistorydata = hongwaiHistorydata;
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
            string nowYear = System.DateTime.Now.Year.ToString();
            string nowMonth = System.DateTime.Now.Month.ToString("00");
            string nowDay = System.DateTime.Now.Day.ToString("00");
            var query = _hongwaidata.FindList(p => p.sn == sn && p.Year == nowYear&&p.Month==nowMonth&&p.Day==nowDay,"",false);
            return Ok(new {
                res = query
            });
        }
        /// <summary>
        /// 根据设备编号获取其当天24小时的进出数量
        /// </summary>
        /// <param name="sn"></param>
        /// <returns></returns>
        [HttpGet("GetTodayCount/{sn}")]
        public IActionResult GetTodayCount(string sn)
        {
            //string today = DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00";
            //int todayStamp = TimeUtils.ConvertToTimeStamps(today);
            string nowYear = System.DateTime.Now.Year.ToString();
            string nowMonth = System.DateTime.Now.Month.ToString("00");
            string nowDay = System.DateTime.Now.Day.ToString("00");

            //var nowH = System.DateTime.Now.Hour;
            //var query = _hongwaidata.FindList(p => p.sn == sn && p.Year == nowYear && p.Month == nowMonth && p.Day == nowDay, "", false);

            //List<string> inlist = new List<string>();
            //List<string> outlist = new List<string>();

            //升序
            var inQuey = _hongwaiHistorydata.FindList(p => p.Year == nowYear && p.Month == nowMonth && p.Day == nowDay && p.sn == sn && p.type == "0", "timeStamp", true);
            var outQuey = _hongwaiHistorydata.FindList(p => p.Year == nowYear && p.Month == nowMonth && p.Day == nowDay && p.sn == sn && p.type == "1", "timeStamp", true);

            return Ok(new
            {
                inList = inQuey,
                outList = outQuey,
            });
            //for (int i = 0; i <= nowH; i++)
            //{
            //    var inQuey = _hongwaiHistorydata.Find(p => p.Year == nowYear && p.Month == nowMonth && p.Day == nowDay & p.Hour == i.ToString("00") && p.Minute == "59" && p.sn == sn && p.type == "0");
            //    var outQuey = _hongwaiHistorydata.Find(p => p.Year == nowYear && p.Month == nowMonth && p.Day == nowDay & p.Hour == i.ToString("00") && p.Minute == "59" && p.sn == sn && p.type == "1");
            //    if (inQuey != null && outQuey != null)
            //    {
            //        inlist.Add(inQuey.count);
            //        outlist.Add(outQuey.count);
            //    }
            //    else
            //    {
            //        inlist.Add("");
            //        outlist.Add("");
            //    }
            //}
        }
    }
 }