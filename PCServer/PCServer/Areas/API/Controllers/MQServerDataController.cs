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
using KVDDDCore.Utils;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;

namespace SHSecurityServer.Controllers
{
    [Produces("application/json")]
    [Route("api/MQServerData")]
    public class MQServerDataController : Controller
    {
        private readonly ILogger _logger;
        private readonly IMQServerDataRepository _mqServerData;
        public readonly IHostingEnvironment _hostingEnviroment;
        public MQServerDataController(IMQServerDataRepository mqServerData, ILogger<MQServerDataController> logger, IHostingEnvironment hostingEnviroment)
        {
            _logger = logger;
            _mqServerData = mqServerData;
            _hostingEnviroment = hostingEnviroment;
        }
        /// <summary>
        /// 获取一段时间范围内的activeMQ通知记录
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        [HttpGet("GetDataTimeRangeList/{startTime}/{endTime}")]
        public IActionResult GetDataTimeRangeList(int startTime,int endTime)
        {
            var query = _mqServerData.FindList(p => p.timeStamp >= startTime && p.timeStamp <= endTime, "", false);

            return Ok(new {
                res=query
            });
        }

        /// <summary>
        /// 遍历一段时间内的未消警的设备消息列表
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        [HttpGet("GetStillAlarmByRange/{start}/{end}")]
        public async Task<IActionResult> GetStillAlarmByRange(string start,string end)
        {
            var s= "2018-01-24";
            int startStamp = TimeUtils.ConvertToTimeStampByZero(start);
            int endStamp = TimeUtils.ConvertToTimeStampByZero(end,1);

            Dictionary<string, int> recordList = new Dictionary<string, int>();
            List<string> stillAlarmNum = new List<string>();
            var query = _mqServerData.FindList(p => p.timeStamp >= startStamp && p.timeStamp < endStamp, "", false);
            foreach (var item in query)
            {
                var count1 = query.Count();
                if (recordList.ContainsKey(item.dsnum))
                {
                    if (item.topicType=="2"|| item.topicType == "4")
                    {
                        recordList[item.dsnum]-=1;
                    }
                    else if (item.topicType == "1" || item.topicType == "3")
                    {
                        recordList[item.dsnum]+=1;
                    }
                }
                else
                {
                    if (item.topicType == "1" || item.topicType == "3")
                    {
                        recordList.Add(item.dsnum, 1);
                    }
                    else if (item.topicType == "2" || item.topicType == "4")
                    {
                        recordList.Add(item.dsnum, 0);
                    }
                }
            }
            //遍历大于0
            foreach (var item in recordList)
            {
                if (item.Value>0)
                {
                    stillAlarmNum.Add(item.Key);
                }
            }
           
            var resList = query.Where(p => stillAlarmNum.Contains(p.dsnum));
            var count = resList.Count();

            string name = start + "-" + end + ".xlsx";
            String path = _hostingEnviroment.WebRootPath+ @"\MQQueryData\" + name;
            if (!Directory.Exists(_hostingEnviroment.WebRootPath + @"\MQQueryData"))
            {
                Directory.CreateDirectory(_hostingEnviroment.WebRootPath + @"\MQQueryData");
            }
            var res = await NPOIHelper.WriteXlsx(path,resList);

            if(res)
            {
                var stream = new FileStream(path, FileMode.Open);
                return File(stream, "application /octet-stream", name);
            } else
            {
                return BadRequest();
            }
        }


    }
 }