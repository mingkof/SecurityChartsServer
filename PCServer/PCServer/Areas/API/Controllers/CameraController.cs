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
    [Route("api/cameras")]
    public class CameraController : Controller
    {
        private readonly ILogger _logger;
        private readonly ICamerasRepository _cameraRepo;
        private readonly ICamePeopleCountRepository _camPeopleCount;
        private readonly ISysConfigRepository _sysConfig;
        public CameraController(ICamerasRepository cameraRepo, ICamePeopleCountRepository camPeopleCount, ISysConfigRepository sysConfig, ILogger<PoliceGpsController> logger)
        {
            _logger = logger;
            _cameraRepo = cameraRepo;
            _camPeopleCount = camPeopleCount;
            _sysConfig = sysConfig;
        }

        [HttpGet("list")]
        public IActionResult GetList()
        {
            var list = _cameraRepo.FindList(p => true,"",false);

            return Ok(new
            {
                res = list
            });
        }

        [HttpGet("getcamera/{id}")]
        public IActionResult GetCamera(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Ok(new
                {
                    res = "{}"
                });
            }
            
            var query = _cameraRepo.Find(p => p.id == id);

            return Ok(new
            {
                res = query
            });
        }
        /// <summary>
        /// 根据图表和镜头获取所在地名和人流量以及url
        /// </summary>
        /// <param name="cameraid">31010820001180001008</param>
        /// <returns></returns>
        [HttpGet("GetCameraVideoInfo/{tableIndex}/{camIndex}")]
        public IActionResult GetCameraVideoInfo(string tableIndex, string camIndex)
        {
            int camKey = 0;
            int urlKey = 0;
            string rspUrl = "";
            string camId = "";
            string title = "";
            string info1 = "";
            string info2 = "";
        
            if (tableIndex != null&& camIndex != null)
            {
                if (tableIndex == "2")
                {

                }
                string camKeystr = "tb" + tableIndex + "_" + "cam" + camIndex + "_id";
                string urlKeystr = "tb" + tableIndex + "_" + "cam" + camIndex + "_url";
                try
                {
                    camKey = (int)Enum.Parse(typeof(EConfigKey), camKeystr);
                    urlKey = (int)Enum.Parse(typeof(EConfigKey), urlKeystr);
                }
                catch 
                {
                    return BadRequest("参数错误");
                }

                var urlquery = _sysConfig.Find(p => p.key == urlKey);
                var camidquery = _sysConfig.Find(p => p.key == camKey);

                if (urlquery != null && camidquery != null)
                {
                    rspUrl = urlquery.value;
                    camId = camidquery.value;
                }
                else
                {
                    return BadRequest("url或者camid无数据");
                }
                var camquery = _cameraRepo.Find(p => p.id == camId);
                var campeoquery = _camPeopleCount.Find(p => p.ID == camId);
                if (camquery!=null&& campeoquery!=null)
                {
                    title = camquery.name;
                    info1 = campeoquery.Count.ToString();
                }

                return Ok(new {
                    res = new {
                        camId = camId,
                        title = title,
                        info1=info1,
                        info2=info2,
                        rspUrl=rspUrl
                    }
                });

            }
            return BadRequest("无数据");
        }

    }
}
