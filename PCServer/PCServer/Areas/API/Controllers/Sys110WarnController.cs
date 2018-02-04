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
    [Route("api/110warn")]
    public class Sys110WarnController : Controller
    {
        private readonly ILogger _logger;
        private readonly ISys110WarningRepository _sys110warnRepository;
        public Sys110WarnController(ISys110WarningRepository sys110warnRepos, ILogger<Sys110WarnController> logger)
        {
            _logger = logger;

            _sys110warnRepository = sys110warnRepos;
        }

        // [HttpGet("alllist", Name = "AllList")]
        // public IActionResult AllList()
        // {
        //     var list = _sys110warnRepository.FindList(p => true, "", false);

        //     if (list == null)
        //         return BadRequest("无数据");

        //     return Ok(new
        //     {
        //         array = list
        //     });
        // }


        /// <summary>
        /// 获取今日报警列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("list", Name = "warnList")]
        public IActionResult Get()
        {
            string nowYear = System.DateTime.Now.Year.ToString();
            string nowMonth = System.DateTime.Now.Month.ToString("00");
            string nowDay = System.DateTime.Now.Day.ToString("00");

            var list = _sys110warnRepository.FindList(p => p.YEAR == nowYear && p.MONTH == nowMonth && p.DAY == nowDay, "", false);

            if (list == null)
                return BadRequest("无数据");

            return Ok(new
            {
                array = list
            });
        }

        /// <summary>
        /// 获取最新时间的警情列表，分页
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("NewerListByPage/{pageIndex}/{pageSize}")]
        public IActionResult NewerListByPage(int pageIndex, int pageSize)
        {
            string nowYear = System.DateTime.Now.Year.ToString();
            string nowMonth = System.DateTime.Now.Month.ToString("00");
            string nowDay = System.DateTime.Now.Day.ToString("00");

            var list = _sys110warnRepository.FindPageList(pageIndex, pageSize,out int totalSize, p => p.YEAR == nowYear && p.MONTH == nowMonth && p.DAY == nowDay, "TIMESIGN", false);

            if (list == null)
                return BadRequest("无数据");

            return Ok(new
            {
                array = list
            });
        }


        [HttpGet("areamsg")]
        public async Task<IActionResult> GetAreaMsg([FromServices] INodeServices nodeServices) {
                        
            var result = await nodeServices.InvokeAsync<string>(
                            "Node/gongan/Query110State.js"
                            );


            _logger.LogTrace("GetAreaMsg: " + result);

            //NAME,FEIZI, FEIMU, INCRESS, CFT
            List<string[]> queryResult = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string[]>>(result);

            if(queryResult == null) {
                return Ok(new
                {
                    array = "err"
                });
            }

            List<Object> list = new List<object>();

            for (int i = 0; i < queryResult.Count(); i++)
            {
                list.Add(new {
                    AreaName = queryResult[i][0],
                    AlldayCount = queryResult[i][4],
                    Present = queryResult[i][3]
                });
            }

            return Ok(new
            {
                array = list
            });
        }


        /// <summary>
        ///  获取某时间段的报警列表
        /// </summary>
        /// <param name="begin">2017-09-25_14:05:00</param>
        /// <param name="end"></param>
        /// <returns></returns>
        [HttpGet("rangelist/{begin}/{end}", Name = "GetRangeList")]
        //[Route("/api/110warn/rangelist")]
        public IActionResult GetRangeList(long begin, long end)
        {
            var list = _sys110warnRepository.FindList(p => p.TIMESIGN >= begin && p.TIMESIGN <= end, "", false);

            return Ok(new
            {
                array = list
            });
        }

        /// <summary>
        /// 获取今日警情数量
        /// </summary>
        /// <returns></returns>
        [HttpGet("count")]
        public IActionResult Count()
        {
            string nowYear = System.DateTime.Now.Year.ToString();
            string nowMonth = System.DateTime.Now.Month.ToString("00");
            string nowDay = System.DateTime.Now.Day.ToString("00");

            var count = _sys110warnRepository.Count(p => p.YEAR == nowYear && p.MONTH == nowMonth && p.DAY == nowDay);
            return Ok(count);
        }


        /// <summary>
        ///   每小时分类获取报警信息     cate: 1 报警类，  2 交通类， 3 事故类， 4 其他
        /// </summary>
        /// <param name="cate"></param>
        /// <returns></returns>
        [HttpGet("countByCate/{cate}/{hour}")]
        public IActionResult CountByCate(string cate,string hour)
        {
            if (string.IsNullOrEmpty(cate))
                cate = "其他";
            
            string nowYear = System.DateTime.Now.Year.ToString();
            string nowMonth = System.DateTime.Now.Month.ToString("00");
            string nowDay = System.DateTime.Now.Day.ToString("00");

            var count=0;
            switch (cate)
            {
                case "1": count = _sys110warnRepository.Count(p => p.YEAR == nowYear && p.MONTH == nowMonth && p.DAY == nowDay && p.HH == hour&& (p.FKAY1.Contains("报警"))); break;
                case "2": count = _sys110warnRepository.Count(p => p.YEAR == nowYear && p.MONTH == nowMonth && p.DAY == nowDay && p.HH == hour && (p.FKAY1.Contains("交通"))); break;
                case "3": count = _sys110warnRepository.Count(p => p.YEAR == nowYear && p.MONTH == nowMonth && p.DAY == nowDay && p.HH == hour && (p.FKAY1.Contains("事故"))); break;
                case "4": count = _sys110warnRepository.Count(p => p.YEAR == nowYear && p.MONTH == nowMonth && p.DAY == nowDay && p.HH == hour && (p.FKAY1.Contains("其他")|| p.FKAY1.Contains("纠纷")|| p.FKAY1.Contains("求助")|| p.FKAY1.Contains("空值"))); break;
                default: break;
            }

            return Ok(new {
                res = count
            });
        }

        /// <summary>
        /// 获取今日每小时警情数
        /// </summary>
        /// <returns></returns>
        [HttpGet("hourcount")]
        public async Task<IActionResult> HourCount()
        {
            var res = await GetHourCount();
            return Ok(res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create([FromBody]sys_110warningdb value)
        {
            if (value == null || value.JJD_ID == null || !ModelState.IsValid)
                return BadRequest("错误请求");

            var query = _sys110warnRepository.GetWarn(value.JJD_ID);

            if (query != null)
                return BadRequest("ID已存在");

            _sys110warnRepository.Add(value);

            return Ok();
        }


        /// <summary>
        /// 清空110警情数据
        /// </summary>
        /// <returns></returns>
        [HttpDelete("clearall")]
        //[Route("/api/110warn/clearall")]
        public IActionResult ClearAll()
        {
            var query = _sys110warnRepository.FindList(p => true, "", false);

            if (query != null)
                _sys110warnRepository.RemoveRange(query);

            return Ok();
        }

        /// <summary>
        /// 根据报警电话获取警情信息
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        [HttpGet("phonelist/{phone}")]
        public IActionResult HourCount(string phone)
        {
            var query = _sys110warnRepository.FindList(p => p.BJ_PHONE == phone, "", false);

            if (query == null)
                return BadRequest("无数据");

            return Ok(new
            {
                array = query
            });
        }
        /// <summary>
        /// 根据关键词获取110警情列表
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        [HttpGet("cmsearch/{word}")]
        public IActionResult CmSearch(string word)
        {
            var query = _sys110warnRepository.FindList(p => p.COMMET.Contains(word), "", false);

            if (query == null)
                return BadRequest("无数据");

            return Ok(new
            {
                array = query
            });
        }

        /// <summary>
        /// 获取今日每小时的警情数据
        /// </summary>
        /// <returns></returns>
        private async Task<Dictionary<int, int>> GetHourCount()
        {
            string nowYear = System.DateTime.Now.Year.ToString();
            string nowMonth = System.DateTime.Now.Month.ToString("00");
            string nowDay = System.DateTime.Now.Day.ToString("00");
            int nowHour = System.DateTime.Now.Hour;

            Dictionary<int, int> hourCounts = new System.Collections.Generic.Dictionary<int, int>();

            for (int i = 0; i <= nowHour; i++)
            {
                var count = _sys110warnRepository.Count(p => p.YEAR == nowYear && p.MONTH == nowMonth && p.DAY == nowDay && p.HH == i.ToString("00"));
                hourCounts.Add(i, count);
            }
            return hourCounts;
        }

        [HttpGet("GetTotalAlarmCount")]
        public IActionResult GetTotalAlarmCount()
        {
            string nowYear = System.DateTime.Now.Year.ToString();
            string nowMonth = System.DateTime.Now.Month.ToString("00");
            string nowDay = System.DateTime.Now.Day.ToString("00");

            var tCount = _sys110warnRepository.Count(p => p.YEAR == nowYear && p.MONTH == nowMonth && p.DAY == nowDay&&p.CJDW=="站区治安派出所");
            var aCount = _sys110warnRepository.Count(p => p.YEAR == nowYear && p.MONTH == nowMonth && p.DAY == nowDay && p.CJDW == "站区治安派出所" && (p.FKAY1.Contains("报警类")));


            return Ok(
                new {
                    toatlCount= tCount,
                    alarmCount= aCount
                }
                );
        }


    }
}
