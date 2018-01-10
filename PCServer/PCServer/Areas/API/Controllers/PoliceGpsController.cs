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
        public PoliceGpsController(IPoliceGpsRepository policeGps, ILogger<PoliceGpsController> logger, ISysConfigRepository configRepo, IPoliceGPSAreaStaticRepository police_area_static_repo)
        {
            _logger = logger;
            _policeGps = policeGps;
            _configRepo = configRepo;
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

        [HttpGet("onlineCount")]
        public IActionResult GetOnlineCount()
        {
            string nowYear = System.DateTime.Now.Year.ToString();
            string nowMonth = System.DateTime.Now.Month.ToString("00");
            string nowDay = System.DateTime.Now.Day.ToString("00");

            var count = _policeGps.Count(p => p.Year == nowYear && p.Month == nowMonth && p.Day == nowDay);

            return Ok(new
            {
                res = count
            });
        }


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

        //设置今日静安总警力
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

        //获取不同区域在今日某小时的警力数，用于警力分布图表
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hour">传入小时</param>
        /// <returns></returns>
        [HttpGet("GetAreaTodayHourPoliceCount/{hour}")]
        public IActionResult GetAreaTodayHourPoliceCount(int hour)
        {
            var DayNow = DateTime.Now;
            string nowYear = DayNow.Year.ToString();
            string nowMonth = DayNow.Month.ToString("00");
            string nowDay = DayNow.Day.ToString("00");
            string nowHour = DayNow.Hour.ToString("00");

            List<string> areas = PCServerMain.Instance.PoliceGpsStaticAreaManager.Areas.Keys.ToList();
            List<int> count = new List<int>();
            for (int i = 0; i < areas.Count; i++)
            {
                var query = _police_area_static_repo.Count(p => p.Year == nowYear && p.Month == nowMonth && p.Day == nowDay && p.HH == nowHour && p.AreaName == areas[i]);
                count.Add(query);
            }

            return Ok(new
            {
                hour = hour,
                areas = areas,
                counts = count
            });
        }

    }
}
