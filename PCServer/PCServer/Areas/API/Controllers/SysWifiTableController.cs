using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SHSecurityContext.IRepositorys;
using SHSecurityModels;
using SHSecurityServer.Models;
using System;
using System.Collections.Generic;

namespace SHSecurityServer.Controllers
{
 



    [Produces("application/json")]
    [Route("api/wifitb")]
    public class SysWifiTableController : Controller
    {
        private readonly ILogger _logger;
        private readonly ISysWifiTableRepository _sysWifiTableRepository;
        public SysWifiTableController(ISysWifiTableRepository sysWifiTableRepository, ILogger<Sys110WarnController> logger)
        {
            _logger = logger;
            _sysWifiTableRepository = sysWifiTableRepository;
        }

        //[HttpGet]
        //public IActionResult Get()
        //{
        //    return Ok(_sysWifiTableRepository.FindList(p => true, "", false));
        //}

        [HttpGet("statics", Name = "GetStatics")]
        //[Route("/api/wifitb/statics")]
        public IActionResult GetStatics()
        {
            string timeNowStr1 = System.DateTime.Now.ToString("HH:mm:ss");
            string timeNowStr2 = System.DateTime.Now.ToString("yyyy-MM-dd");
            int timestampNow = KVDDDCore.Utils.TimeUtils.ConvertToTimeStampNow();
            int timestampPre = timestampNow - 5 * 60;

            _logger.LogInformation("GetStatics:" + timestampNow + "  " + timestampPre);


            List<WifiStatics> list = new List<WifiStatics>();

            int totalCount = 0;
            for (int i = 0; i < SampleData.WifiIdList.Count; i++)
            {
                int count = _sysWifiTableRepository.Count(p => p.COLLECTION_EQUIPMENT_ID == SampleData.WifiIdList[i] && int.Parse(p.CAPTURE_TIME) > timestampPre);
                list.Add(new WifiStatics()
                {
                    COLLECTION_EQUIPMENT_ID = SampleData.WifiIdList[i],
                    AllDeviceCount = count
                });

                totalCount += count;
            }
                                                                                                                                                                             
            Random rd = new Random();

            return Ok(new {
                array = list,
                requestTime = timestampNow,
                requestTimeStr1 = timeNowStr1,
                requestTimeStr2 = timeNowStr2,
                totalCount = rd.Next(0, 1000)
            });
        }

        [HttpPost]
        public IActionResult Create([FromBody]sys_wifitable value)
        {
            if (value == null || !ModelState.IsValid)
                return BadRequest("错误请求");

            _sysWifiTableRepository.Add(value);

            return Ok();
        }

        /// <summary>
        /// 清楚所有数据
        /// </summary>
        /// <returns></returns>
        [HttpDelete("clearall", Name = "ClearAll")]
        //[Route("/api/wifitb/clearall")]
        public IActionResult ClearAll()
        {
            var query = _sysWifiTableRepository.FindList(p => true, "", false);

            if (query != null)
                _sysWifiTableRepository.RemoveRange(query);

            return Ok();
        }

    }
 } 

