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
    [Route("api/wifidatapeople")]
    public class WifiDataPeoplesController : Controller
    {
        private readonly ILogger _logger;
        private readonly IWifiDataPeoplesHistoryRepository _wifidata_history;
        private readonly IWifiDataPeoplesRepository _wifidata;
        public WifiDataPeoplesController(IWifiDataPeoplesHistoryRepository wifidata_history, IWifiDataPeoplesRepository wifidata,ILogger<Sys110WarnController> logger)
        {
            _logger = logger;
            _wifidata = wifidata;
            _wifidata_history = wifidata_history;
        }

        /// <summary>
        /// 获取当天24小时内, 该区域的数量总和
        /// </summary>
        /// <param name="areaid">根据static/WifiDataAreas.json配置获得</param>
        /// <returns></returns>
        [HttpGet("GetAreaWifiData/{areaid}")]
        public IActionResult GetAreaWifiData(int areaid)
        {
            var areas = PCServerMain.Instance.WifiDataAreas.data.Where(p => p.id == areaid).FirstOrDefault();
            if(areas == null)
            {
                return BadRequest(new { message = "Cant Find AreaID!" });
            }

            var YEAR = System.DateTime.Now.Year.ToString();
            var MONTH = System.DateTime.Now.Month.ToString("00");
            var DAY = System.DateTime.Now.Day.ToString("00");
            var HH = System.DateTime.Now.Hour.ToString("00");
            var MM = System.DateTime.Now.Minute.ToString("00");
            var SS = System.DateTime.Now.Second.ToString("00");

            Dictionary<string, int> HoursToCount = new Dictionary<string, int>();

            for (int i = 0; i < 24; i++)
            {
                string hhformat = i.ToString("00");
                var qlist = _wifidata_history.FindList(p => p.Year == YEAR && p.Month == MONTH && p.Day == DAY && p.HH == hhformat && areas.ids.Contains(p.WifiID),"",false);

                var count = 0;
                if(qlist != null) {
                    count = qlist.Sum(p => p.Count);
                }

                HoursToCount.Add("H"+i, count);
            }

            return Ok(new
            {
                res = HoursToCount
            });
        }

    }
 } 

