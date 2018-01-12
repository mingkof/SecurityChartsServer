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
    [Route("api/RoadDataRecord")]
    public class RoadDataRecordController : Controller
    {
        private readonly ILogger _logger;
        private readonly IRoadDataRecordRepository _roadDataRecord;

        public RoadDataRecordController(IRoadDataRecordRepository roadDataRecord, ILogger<Sys110WarnController> logger)
        {
            _logger = logger;
            _roadDataRecord = roadDataRecord;
        }

        //根据路名获取其24之内的平均交通指数
        [HttpGet("GetTodayHoursAvgData/{roadName}/")]
        public IActionResult GetTodayHoursAvgData(string roadName)
        {
            var YEAR = System.DateTime.Now.Year.ToString();
            var MONTH = System.DateTime.Now.Month.ToString("00");
            var DAY = System.DateTime.Now.Day.ToString("00");
            var HH = System.DateTime.Now.Hour;

            Dictionary<string, float> HoursToData = new Dictionary<string, float>();

            for (int i = 0; i <= (int)HH; i++)
            {

                var query=_roadDataRecord.FindList(p=>p.Roadname==roadName&&p.Year==YEAR&&p.Month==MONTH&&p.Day==DAY&&p.HH==i.ToString("00"),"",false).ToList<RoadDataRecord>();
                if (query!=null)
                {
                    float sumCount=0;
                    for (int m = 0; m < query.Count; m++)
                    {
                        float.TryParse(query[m].TrafficData,out float trafficData);
                        sumCount+=trafficData;
                    }
                    if(query.Count==0)
                        HoursToData.Add(i.ToString("00"),0);
                    else
                        HoursToData.Add(i.ToString("00"),sumCount/query.Count);
                }
            }

            return Ok(new {
                res=HoursToData
            });
        }
        //获取最新的道路信息
        [HttpGet("GetRoadRealData/{roadName}/")]
        public IActionResult GetRoadRealData(string roadName)
        {
            var query=_roadDataRecord.FindList(p=>p.Roadname==roadName,"Timestamp",false);

            if (query!=null)
            {
                var result=query.ToList()[0];
                 return Ok(new {
                        res=result
                    });
            }

            return BadRequest();
        }

    }
 }