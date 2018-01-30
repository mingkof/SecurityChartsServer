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
            var area = PCServerMain.Instance.WifiDataAreas.data.Where(p => p.id == areaid).FirstOrDefault();
            if(area == null)
            {
                return BadRequest(new { message = "Cant Find AreaID!" });
            }

            var YEAR = System.DateTime.Now.Year.ToString();
            var MONTH = System.DateTime.Now.Month.ToString("00");
            var DAY = System.DateTime.Now.Day.ToString("00");
            var HHInt = DateTime.Now.Hour;

            var HH = HHInt.ToString("00");
            var MM = System.DateTime.Now.Minute.ToString("00");
            var SS = System.DateTime.Now.Second.ToString("00");

            Dictionary<string, int> HoursToCount = new Dictionary<string, int>();

            for (int i = 0; i <= HHInt; i++)
            {
                bool shouldQuery = false;
                var count = 0;

                //判断缓存
                if (i != HHInt)
                {
                    if(area.history.ContainsKey(i))
                    {
                        count = area.history[i];
                    } else
                    {
                        shouldQuery = true;
                    }
                } else
                {
                    shouldQuery = true;
                }

                if(shouldQuery)
                {
                    string hhformat = i.ToString("00");
                    var qlist = _wifidata_history.FindList(p => p.Year == YEAR && p.Month == MONTH && p.Day == DAY && p.HH == hhformat && area.ids.Contains(p.WifiID), "", false);

                    if (qlist != null)
                    {
                        count = qlist.Sum(p => p.Count);
                    }
                }

                HoursToCount.Add("H"+i, count);

                if(shouldQuery)
                {
                    if(area.history.ContainsKey(i))
                    {
                        area.history[i] = count;
                    } else
                    {
                        area.history.Add(i, count);
                    }
                }
            }

            return Ok(new
            {
                res = HoursToCount
            });
        }

        /// <summary>
        ///  获取当天24小时内,南北客流量统计
        /// </summary>
        /// <param name="sn">南北  南0 北1</param>
        /// <returns></returns>
        [HttpGet("GetSNPassengerCount/{sn}")]
        public IActionResult GetSNPassengerCount(int  sn)
        {
            var area = PCServerMain.Instance.WifiDataAreas.data.Where(p => p.id == 1).FirstOrDefault();
            if (area == null)
            {
                return BadRequest(new { message = "Cant Find AreaID!" });
            }

            var YEAR = System.DateTime.Now.Year.ToString();
            var MONTH = System.DateTime.Now.Month.ToString("00");
            var DAY = System.DateTime.Now.Day.ToString("00");
            var HHInt = DateTime.Now.Hour;

            var HH = HHInt.ToString("00");
            var MM = System.DateTime.Now.Minute.ToString("00");
            var SS = System.DateTime.Now.Second.ToString("00");

            Dictionary<string, int> HoursToCount = new Dictionary<string, int>();
            List<int> countList = new List<int>();
            var wifiConfig = PCServerMain.Instance.wifiConfigDic;

            for (int i = 0; i <= HHInt; i++)
            {
                bool shouldQuery = true;
                var count = 0;
                IQueryable<wifidata_peoples_history> qlist = null;
                if (shouldQuery)
                {
                    string hhformat = i.ToString("00");
                    if (sn==0)
                    {
                        qlist = _wifidata_history.FindList(p => p.Year == YEAR && p.Month == MONTH && p.Day == DAY && p.HH == hhformat && wifiConfig["南广场"].Contains(p.WifiID), "", false);
                    }
                    else if (sn==1)
                    {
                        qlist = _wifidata_history.FindList(p => p.Year == YEAR && p.Month == MONTH && p.Day == DAY && p.HH == hhformat && wifiConfig["北广场"].Contains(p.WifiID), "", false);
                    }

                    if (qlist != null)
                    {
                        count = qlist.Sum(p => p.Count);
                    }
                }

                //countList.Add(new Random().Next(0,500));
                countList.Add(count);
            }

            return Ok(new
            {
                res = countList
            });
        }




        /// <summary>
        /// 获取南北广场客流量
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetPeopleCount")]
        public IActionResult GetPeopleCount()
        {
            var wifiConfig = PCServerMain.Instance.wifiConfigDic;
            var sCount = 0;
            var nCount = 0;
            if (wifiConfig!=null)
            {
                if (wifiConfig["南广场"]!=null)
                {
                    var sQuery = _wifidata.FindList(p => wifiConfig["南广场"].Contains(p.WifiID),"",false);
                    sCount = sQuery.Sum(p => p.Count);
                }
                if (wifiConfig["北广场"] != null)
                {
                    var nQuery = _wifidata.FindList(p => wifiConfig["北广场"].Contains(p.WifiID), "", false);
                    nCount = nQuery.Sum(p => p.Count);
                }
            }

            return Ok(new {
                res=new {
                    southCount= sCount,
                    northCount= nCount
                }
            });
        }
    }
 } 

