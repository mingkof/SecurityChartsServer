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

namespace SHSecurityServer.Controllers
{
    [Produces("application/json")]
    [Route("api/FaceAlarmData")]
    public class FaceAlarmDataController : Controller
    {
        private readonly ILogger _logger;
        private readonly IFaceAlarmDataRepositoy _faceAlarmData;
        private RealDataUrl RealDataUrlConfig;
        public FaceAlarmDataController(IFaceAlarmDataRepositoy faceAlarmData,ILogger<FaceAlarmDataController> logger, IOptions<RealDataUrl> config)
        {
            _logger = logger;
            _faceAlarmData = faceAlarmData;
            RealDataUrlConfig = config.Value;
        }
        /// <summary>
        /// 分页获取人脸识别信息
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("GetFaceItemData/{pageIndex}/{pageSize}")]
        public IActionResult GetAlarmItemData(int pageIndex, int pageSize)
        {
            string today = DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00";
            int todayStamp = TimeUtils.ConvertToTimeStamps(today);
            var list = _faceAlarmData.FindPageList(pageIndex, pageSize, out int totalSize,p=>p.timeStamp > todayStamp, "", true);
            return Ok(new {
                res=list
            });
        }

        /// <summary>
        /// 获取人脸识别照片
        /// </summary>
        /// <param name="alarmId">人脸识别告警ID</param>
        /// <param name="type"> 0 : 全景  1：脸部</param>
        /// <returns></returns>
        [HttpGet("GetAlarmHumanImg/{type}/{alarmId}")]
        public IActionResult GetAlarmHumanImg(int type, string alarmId)
        {
            FtpClient ftpClient = new FtpClient(RealDataUrlConfig.ip, RealDataUrlConfig.username, RealDataUrlConfig.userpassword);
            Stream stream =null;
            if (type==0)
            {
                stream = ftpClient.Download("AlarmData/" + alarmId + "/pics/9f28d5b36adc48f69b3fff19f2f3eeb7_face.png");
            }
            else if (type == 1)
            {
                stream = ftpClient.Download("AlarmData/" + alarmId + "/pics/9f28d5b36adc48f69b3fff19f2f3eeb7_bkg.png");
            }
            if (stream != null)
                return File(stream, "image/png");
            else
                return null;
        }

        /// <summary>
        /// 获取匹配人脸的照片
        /// </summary>
        /// <param name="alarmId"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpGet("GetAlarmFatchHumanImg/{alarmId}/{humanid}")]
        public IActionResult GetAlarmFatchHumanImg(string alarmId, string humanid)
        {
            FtpClient ftpClient = new FtpClient(RealDataUrlConfig.ip, RealDataUrlConfig.username, RealDataUrlConfig.userpassword);
            var stream = ftpClient.Download("AlarmData/" + alarmId + "/pics/humans/"+ humanid + "/"+humanid+"_face.png");
            if (stream != null)
                return File(stream, "image/png");
            else
                return null;
        }


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

