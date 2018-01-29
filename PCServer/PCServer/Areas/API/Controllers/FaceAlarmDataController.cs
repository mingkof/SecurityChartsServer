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
using Microsoft.Extensions.Options;
using MKServerWeb.Model.RealData;
using KVDDDCore.Utils;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace SHSecurityServer.Controllers
{
    [Produces("application/json")]
    [Route("api/FaceAlarmData")]
    public class FaceAlarmDataController : Controller
    {
        private readonly ILogger _logger;
        private readonly IFaceAlarmDataRepositoy _faceAlarmData;
        private readonly ISysTicketresRepository _sysTicketresRepository;

        private readonly IHostingEnvironment _hostingEnv;
        private RealDataUrl RealDataUrlConfig;
        public FaceAlarmDataController(IFaceAlarmDataRepositoy faceAlarmData, ISysTicketresRepository sysTicketresRepository, IHostingEnvironment hostingEnv, ILogger<FaceAlarmDataController> logger, IOptions<RealDataUrl> config)
        {
            _logger = logger;
            _faceAlarmData = faceAlarmData;
            _sysTicketresRepository = sysTicketresRepository;
            RealDataUrlConfig = config.Value;
            _hostingEnv = hostingEnv;
        }
        /// <summary>
        /// 获取今日人脸识别重点人员报警次数
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetTodayCount")]
        public IActionResult GetTodayCount()
        {
            string today = DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00";
            int todayStamp = TimeUtils.ConvertToTimeStamps(today);
            var count = _faceAlarmData.Count(p => p.timeStamp > todayStamp);
            return Ok(new
            {
                res = count
            });
        }

        /// <summary>
        /// 获取历史所有人脸识报警次数
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetHistoryAllCount")]
        public IActionResult GetHistoryAllCount()
        {
            var count = _faceAlarmData.Count(p =>true);
            return Ok(new
            {
                res = count
            });
        }

        /// <summary>
        /// 分页获取重点人员人脸识别信息
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("GetFaceItemData/{pageIndex}/{pageSize}")]
        public IActionResult GetAlarmItemData(int pageIndex, int pageSize)
        {
            string today = DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00";
            int todayStamp = TimeUtils.ConvertToTimeStamps(today);
            var list = _faceAlarmData.FindPageList(pageIndex, pageSize, out int totalSize,p=>p.timeStamp > todayStamp,"timeStamp", false);
            return Ok(new {
                res=list
            });
        }

        /// <summary>
        /// 获取重点人员人脸识别照片
        /// </summary>
        /// <param name="alarmId">人脸识别告警ID</param>
        /// <param name="type"> 0 : 脸部  1：全景</param>
        /// <returns></returns>
        [HttpGet("GetAlarmHumanImg/{type}/{alarmId}")]
        public IActionResult GetAlarmHumanImg(int type, string alarmId)
        {
            string path = "/FaceAlarmData/AlarmData/" + alarmId + "/pics/";
            //Stream stream =null;
            var query= _faceAlarmData.Find(p => p.alarmId == alarmId);

            string id = "";
            if (query != null)
                id = query.humanId;

            string imgUrl = "";

            if (type==0)
            {
                imgUrl = path + id + "_face.png";
                //stream = FileUtils.ReadFileToStream(path + id + "_face.png");
                //stream = FileUtils.ReadFileToStream("static/baidu.jpg");
            }
            else if (type == 1)
            {
                imgUrl = path + id + "_bkg.png";
                //stream = FileUtils.ReadFileToStream(path + id + "_bkg.png");
            }

            return Ok(new
            {
                res = imgUrl
            });
            //if (stream != null)
            //    return File(stream, "image/png");
            //else
            //    return null;
        }

        /// <summary>
        /// 获取重点人员匹配人脸的照片
        /// </summary>
        /// <param name="alarmId"></param>
        /// <param name="humanid"></param>
        /// <returns></returns>
        [HttpGet("GetAlarmMatchHumanImg/{alarmId}/{humanid}")]
        public IActionResult GetAlarmMatchHumanImg(string alarmId, string humanid)
        {
            //var stream = FileUtils.ReadFileToStream(_hostingEnv.WebRootPath + @"\FaceAlarmData\AlarmData\"+ alarmId + @"\pics\humans\"+ humanid + @"\"+humanid+"_face.png");
            //if (stream != null)
            //    return File(stream, "image/png");
            //else
            //    return null;

            string imgUrl = "";
            imgUrl = "/FaceAlarmData/AlarmData/" + alarmId + "/pics/humans/" + humanid + "/" + humanid + "_face.png";
            return Ok(new
            {
                res = imgUrl
            });
        }
        /// <summary>
        /// 获取重点人员匹配人员列表
        /// </summary>
        /// <param name="alarmId"></param>
        /// <returns></returns>
        [HttpGet("GetAlarmFatchHumanNameList/{alarmId}")]
        public IActionResult GetAlarmFatchHumanNameList(string alarmId)
        {
            List<string> list = new List<string>();

            var query = _faceAlarmData.Find(p => p.alarmId == alarmId);
            if (query!=null)
            {
                list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(query.matchHumanList);
            }
            return Ok(new
            {
                res = list
            });
        }
        /// <summary>
        /// 获取今日 车辆报警 人脸识别报警  客运报警的数量
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAlarmTodayCount")]
        public IActionResult GetAlarmTodayCount()
        {
            string today = DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00";
            int todayStamp = TimeUtils.ConvertToTimeStamps(today);
            var faceCount = _faceAlarmData.Count(p => p.timeStamp >= todayStamp);
            var ticketCount= _sysTicketresRepository.Count(p => TimeUtils.ConvertToTimeStamps(p.TicketDate) > todayStamp);
            var carCount = 556;
            return Ok(new {
                faceAlarmCount=faceCount,
                ticketAlarmCount=ticketCount,
                carAlarmCount=carCount
            });
        }

        /// <summary>
        /// 获取历史所有 车辆报警 人脸识别报警  客运报警的数量
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetHistoryAlarmCount")]
        public IActionResult GetHistoryAlarmCount()
        {
            var faceCount = _faceAlarmData.Count(p => true);
            var ticketCount = _sysTicketresRepository.Count(p => true);
            var carCount = 1556;
            return Ok(new
            {
                faceHistoryCount = faceCount,
                ticketHistoryCount = ticketCount,
                carHistoryCount = carCount
            });
        }


        /// <summary>
        /// 获取 月累计  月平均 车辆报警 人脸识别报警  客运报警的数量
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetMonthAlarmCount")]
        public IActionResult GetMonthAlarmCount()
        {
            var monthInt = DateTime.Now.Month;

            var year = DateTime.Now.Year.ToString();
            var month = DateTime.Now.Month.ToString("00");
            var day = DateTime.Now.Day.ToString("00");

            var faceMonthCount = _faceAlarmData.Count(p => p.Year == year && p.Month == month);

            var faceAvgCount =0 ;
            for (int i = 1; i <= monthInt; i++)
            {
                var count = _faceAlarmData.Count(p => p.Year == year && p.Month == i.ToString("00"));
                faceAvgCount += count;
            }
            faceAvgCount = faceAvgCount / monthInt;


            int monthStamp = TimeUtils.ConvertToTimeStamps(DateTime.Now.ToString("yyyy-MM")+"-01 00:00:00");

            var ticketMonthCount = _sysTicketresRepository.Count(p=> TimeUtils.ConvertToTimeStamps(p.TicketDate) > monthStamp);

            var ticketAvgCount = 0;
            var yearStr = year + "-01-01 00:00:00";
            int yearStamp = TimeUtils.ConvertToTimeStamps(yearStr);
            for (int i = 1; i <= monthInt; i++)
            {
                var count = _sysTicketresRepository.Count(p => TimeUtils.ConvertToTimeStamps(p.TicketDate) > yearStamp);
                ticketAvgCount += count;
            }
            ticketAvgCount = ticketAvgCount / monthInt;


            return Ok(new MonthAlarmResult{
                faceMonthCount=faceMonthCount,
                faceAvgCount=faceAvgCount,
                ticketMonthCount=ticketMonthCount,
                ticketAvgCount=ticketAvgCount,
                carMonthCount=5000,
                carAvgCount=5000
        });
        }


    }
 } 

