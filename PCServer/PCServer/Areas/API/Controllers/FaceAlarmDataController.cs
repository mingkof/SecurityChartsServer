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
        private readonly IHostingEnvironment _hostingEnv;
        private RealDataUrl RealDataUrlConfig;
        public FaceAlarmDataController(IFaceAlarmDataRepositoy faceAlarmData, IHostingEnvironment hostingEnv, ILogger<FaceAlarmDataController> logger, IOptions<RealDataUrl> config)
        {
            _logger = logger;
            _faceAlarmData = faceAlarmData;
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
            var list = _faceAlarmData.FindPageList(pageIndex, pageSize, out int totalSize,p=>p.timeStamp > todayStamp,"timeStamp", true);
            return Ok(new {
                res=list
            });
        }

        /// <summary>
        /// 获取重点人员人脸识别照片
        /// </summary>
        /// <param name="alarmId">人脸识别告警ID</param>
        /// <param name="type"> 0 : 全景  1：脸部</param>
        /// <returns></returns>
        [HttpGet("GetAlarmHumanImg/{type}/{alarmId}")]
        public IActionResult GetAlarmHumanImg(int type, string alarmId)
        {
            string path = _hostingEnv.WebRootPath + @"\FaceAlarmData\AlarmData\" + alarmId + @"\pics\" ;
            Stream stream =null;
            var query= _faceAlarmData.Find(p => p.alarmId == alarmId);

            string id = "";
            if (query != null)
                id = query.humanId;
            if (type==0)
            {
                stream = FileUtils.ReadFileToStream(path + id + "_face.png");
                //stream = FileUtils.ReadFileToStream("static/baidu.jpg");
            }
            else if (type == 1)
            {
                stream = FileUtils.ReadFileToStream(path + id + "_bkg.png");
            }
            if (stream != null)
                return File(stream, "image/png");
            else
                return null;
        }

        /// <summary>
        /// 获取重点人员匹配人脸的照片
        /// </summary>
        /// <param name="alarmId"></param>
        /// <param name="humanid"></param>
        /// <returns></returns>
        [HttpGet("GetAlarmFatchHumanImg/{alarmId}/{humanid}")]
        public IActionResult GetAlarmFatchHumanImg(string alarmId, string humanid)
        {
            var stream = FileUtils.ReadFileToStream(_hostingEnv.WebRootPath + @"\FaceAlarmData\AlarmData\"+ alarmId + @"\pics\humans\"+ humanid + @"\"+humanid+"_face.png");
            if (stream != null)
                return File(stream, "image/png");
            else
                return null;
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

    }
 } 

