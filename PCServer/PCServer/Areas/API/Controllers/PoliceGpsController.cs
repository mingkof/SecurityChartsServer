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
using PCServer.Server.GPS;
using PCServer;
using KVDDDCore.Utils;

namespace SHSecurityServer.Controllers
{
    [Produces("application/json")]
    [Route("api/policegps")]
    public class PoliceGpsController : Controller
    {
        private readonly ILogger _logger;
        private readonly IPoliceGpsRepository _policeGps;
        private readonly ISysConfigRepository _configRepo;
        private readonly IPoliceGPSAreaStaticRepository _police_area_static_repo;
        private readonly ISysPoliceAreaHistoryRepository _police_area_history;
        private readonly ISysPoliceAreaRepository _police_area;

        public PoliceGpsController(IPoliceGpsRepository policeGps, ISysPoliceAreaRepository police_area, ISysPoliceAreaHistoryRepository police_area_history, ILogger<PoliceGpsController> logger, ISysConfigRepository configRepo, IPoliceGPSAreaStaticRepository police_area_static_repo)
        {
            _logger = logger;
            _policeGps = policeGps;
            _configRepo = configRepo;
            _police_area_history = police_area_history;
            _police_area = police_area;
            _police_area_static_repo = police_area_static_repo;
        }

        /// <summary>
        /// 得到今天的所有警员gps信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("list")]
        public IActionResult GetList()
        {
            string nowYear = System.DateTime.Now.Year.ToString();
            string nowMonth = System.DateTime.Now.Month.ToString("00");
            string nowDay = System.DateTime.Now.Day.ToString("00");

            var list = _policeGps.FindList(p => p.Year == nowYear && p.Month == nowMonth && p.Day == nowDay, "",false);

            return Ok(new
            {
                PoliceArray = list
            });
        }
        /// <summary>
        /// 获取在岗警力数量
        /// </summary>
        /// <returns></returns>
        [HttpGet("onlineCount")]
        public IActionResult GetOnlineCount()
        {
            var query = _police_area.Find(p => p.AreaName=="火车站整体");
            var count = 0;
            if (query!=null)
            {
                int.TryParse(query.Count,out int num);
                count = num;
            }
            return Ok(new
            {
                res = count
            });
        }

        /// <summary>
        /// 获取今日静安总警力
        /// </summary>
        /// <returns></returns>
        [HttpGet("totalCount")]
        public IActionResult GetTotalCount()
        {
            int totalCount = 0;

            var total = _configRepo.Find(p => p.key == (int)SHSecurityModels.EConfigKey.kPoliceTotalCountTaday);
            if(total != null)
            {
                totalCount = total.valueInt;
            }

            return Ok(new
            {
                res = totalCount
            });
        }

        /// <summary>
        /// 设置今日静安总警力
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        [HttpPost("setTotalCount/{count}")]
        public IActionResult SetTotalCount(int count)
        {
            var total = _configRepo.Find(p => p.key == (int)SHSecurityModels.EConfigKey.kPoliceTotalCountTaday);
            if (total == null)
            {
                total = new sys_config()
                {
                    key = (int)SHSecurityModels.EConfigKey.kPoliceTotalCountTaday,
                    value = "",
                    valueInt = count
                };
                _configRepo.Add(total);
            } else
            {
                total.valueInt = count;
                _configRepo.Update(total);
            }

            return Ok();
        }

        /// <summary>
        /// 获取当前警力分布数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAreaTodayHourPoliceCount/")]
        public IActionResult GetAreaTodayHourPoliceCount()
        {
            var DayNow = DateTime.Now;
            string nowYear = DayNow.Year.ToString();
            string nowMonth = DayNow.Month.ToString("00");
            string nowDay = DayNow.Day.ToString("00");
            string nowHour = DayNow.Hour.ToString("00");
            try
            {
                List<string> areas = new List<string>() { "北广场东北", "北广场东南", "北广场西", "南广场", "其他" };
                List<int> count = new List<int>();
                for (int i = 0; i < areas.Count; i++)
                {
                    var query = _police_area.Find(p => p.AreaName == areas[i]);
                    //count.Add(new Random().Next(0, 20));
                    if (query != null)
                    {
                        int.TryParse(query.Count, out int num);
                        count.Add(num);
                    }
                    else
                    {
                        count.Add(0);
                    }
                }
                return Ok(new
                {
                    areas = areas,
                    counts = count
                });
            }
            catch 
            {
            }
            return BadRequest();
        }
        /// <summary>
        /// 获取每小时的在岗警力
        /// </summary>
        /// <param name="hour"></param>
        /// <returns></returns>
        [HttpGet("GetHourCount/{hour}")]
        public IActionResult GetHourCount(string hour)
        {
            
            var DayNow = DateTime.Now;
            string nowYear = DayNow.Year.ToString();
            string nowMonth = DayNow.Month.ToString("00");
            string nowDay = DayNow.Day.ToString("00");

            var query = _policeGps.Count(p => p.Year == nowYear && p.Month == nowMonth && p.Day == nowDay && p.HH == hour);
            return Ok(new {
                res =query
                });
        }
        /// <summary>
        /// 设置警员所在区域
        /// </summary>
        /// <param name="list">警员id列表</param>
        /// <param name="areaName">区域名：火车站入口  北广场东南  北广场西  北广场东北  其它</param>
        /// <returns></returns>
        [HttpPost("SetPoliceGpsArea/{count}/{areaName}")]
        public IActionResult SetPoliceGpsArea(int count,string areaName)
        {
            var timeStamp = TimeUtils.ConvertToTimeStampNow();
            var DayNow = DateTime.Now;
            string nowYear = DayNow.Year.ToString();
            string nowMonth = DayNow.Month.ToString("00");
            string nowDay = DayNow.Day.ToString("00");
            string nowHour= DayNow.Hour.ToString("00");
            string nowMinute = DayNow.Minute.ToString("00");
            string nowSecond = DayNow.Second.ToString("00");

            try
            {
                var query = _police_area.Find(p => p.AreaName == areaName);
                if (query == null)
                {
                    query = new sys_policearea
                    {
                        AreaName = areaName,
                        Count = count.ToString(),
                        TimeStamp = timeStamp
                    };
                    _police_area.Add(query);
                }
                else
                {
                    query.Count = count.ToString();
                    query.TimeStamp = timeStamp;
                    _police_area.Update(query);
                }

                var queryHistory = _police_area_history.Find(p => p.TimeStamp == timeStamp);
                if (queryHistory == null)
                {
                    queryHistory = new sys_policeareahistory
                    {
                        AreaName = areaName,
                        Count = count.ToString(),
                        TimeStamp = timeStamp,
                        Year = nowYear,
                        Month = nowMonth,
                        Day = nowDay,
                        Hour = nowHour,
                        Minute = nowMinute,
                        Second = nowSecond
                    };
                    _police_area_history.Add(queryHistory);
                }
                else
                {
                    queryHistory.Count = count.ToString();
                    _police_area_history.Update(queryHistory);
                }
            }
            catch 
            {
            }
          
            return Ok();
        }
    }
}
