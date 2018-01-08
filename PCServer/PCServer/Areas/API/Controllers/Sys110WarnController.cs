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



        [HttpGet("list", Name = "warnList")]
        //[Route("/api/110warn/list")]
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
        

        //获取最新时间的警情列表，分页
        [HttpGet("NewerListByPage/{pageIndex}/{pageSize}")]
        public IActionResult NewerListByPage(int pageIndex, int pageSize)
        {
            string nowYear = System.DateTime.Now.Year.ToString();
            string nowMonth = System.DateTime.Now.Month.ToString("00");
            string nowDay = System.DateTime.Now.Day.ToString("00");

            var list = _sys110warnRepository.FindPageList(pageIndex, pageSize,out int totalSize, p => p.YEAR == nowYear && p.MONTH == nowMonth && p.DAY == nowDay, "TIMESIGN", true);

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
        /// 
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


        [HttpGet("count")]
        //[Route("/api/110warn/count")]
        public IActionResult Count()
        {
            string nowYear = System.DateTime.Now.Year.ToString();
            string nowMonth = System.DateTime.Now.Month.ToString("00");
            string nowDay = System.DateTime.Now.Day.ToString("00");

            var count = _sys110warnRepository.Count(p => p.YEAR == nowYear && p.MONTH == nowMonth && p.DAY == nowDay);
            return Ok(count);
        }


        /// <summary>
        /// 分类获取警情数量  
        /// </summary>
        /// <param name="cate">cate: 1 报警类，  2 交通类， 3 事故类， 4 其他</param>
        /// <returns></returns>
        [HttpGet("countByCate/{cate}")]
        public IActionResult CountByCate(int cate)
        {
            Random r = new Random();
            return Ok(r.Next(0,200));

            //if (string.IsNullOrEmpty(cate))
            //    cate = "其他";

            //string nowYear = System.DateTime.Now.Year.ToString();
            //string nowMonth = System.DateTime.Now.Month.ToString("00");
            //string nowDay = System.DateTime.Now.Day.ToString("00");

            ////TODO: 修改为如果是其他， 则分类不等于 报警类， 交通类， 事故类
            //if(cate == "其他")
            //{
            //    var count1 = _sys110warnRepository.Count(p => p.YEAR == nowYear && p.MONTH == nowMonth && p.DAY == nowDay );
            //    return Ok(count1);
            //}

            //var count = _sys110warnRepository.Count(p => p.YEAR == nowYear && p.MONTH == nowMonth && p.DAY == nowDay && p.FKAY1 == cate);
            //return Ok(count);
        }


        [HttpGet("hourcount")]
        //[Route("/api/110warn/hourcount")]
        public async Task<IActionResult> HourCount()
        {
            var res = await GetHourCount();
            return Ok(res);
        }


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



        [HttpDelete("clearall")]
        //[Route("/api/110warn/clearall")]
        public IActionResult ClearAll()
        {
            var query = _sys110warnRepository.FindList(p => true, "", false);

            if (query != null)
                _sys110warnRepository.RemoveRange(query);

            return Ok();
        }


        [HttpGet("phonelist/{phone}")]
        //[Route("/api/110warn/hourcount")]
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

        [HttpGet("cmsearch/{word}")]
        //[Route("/api/110warn/hourcount")]
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


        private async Task<Dictionary<int, int>> GetHourCount()
        {
            string nowYear = System.DateTime.Now.Year.ToString();
            string nowMonth = System.DateTime.Now.Month.ToString("00");
            string nowDay = System.DateTime.Now.Day.ToString("00");
            int nowHour = System.DateTime.Now.Hour;

            Dictionary<int, int> hourCounts = new System.Collections.Generic.Dictionary<int, int>();

            for (int i = 0; i <= nowHour; i++)
            {
                var count = _sys110warnRepository.Count(p => p.YEAR == nowYear && p.MONTH == nowMonth && p.DAY == nowDay && p.HH == i.ToString());
                hourCounts.Add(i, count);
            }

            return hourCounts;
        }




    }
}
