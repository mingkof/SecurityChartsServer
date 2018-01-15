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
using System.Numerics;

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
                    return BadRequest();
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
                    return BadRequest();
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
            return BadRequest();
        }


        /// <summary>
        /// 获取摄像机一定圆形范围内的所有摄像头list
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCameraRangeList/{lang}/{lat}/{radiu}")]
        public IActionResult GetCameraRangeList(string lang, string lat, float radiu)
        {
            if (string.IsNullOrEmpty(lang) || string.IsNullOrEmpty(lat))
            {
                return BadRequest();
            }

            PCServer.Server.GPS.Vector3 centerPos = GPSUtils.ComputeLocalPositionGCJ(lang, lat);
            System.Numerics.Vector3 center = new System.Numerics.Vector3(centerPos.x, centerPos.y, 0);
            //距离小于等于半径
            List<sys_cameras> query = _cameraRepo.FindList(p => true, "", false).ToList<sys_cameras>();

            List<string> cameraList = new List<string>();
            for (int i = 0; i < query.Count; i++)
            {
                var item = query[i];
                float.TryParse(item.worldX,out float x);
                float.TryParse(item.worldY, out float y);

                System.Numerics.Vector3 pos = new System.Numerics.Vector3(x, y, 0);
                if (CheckInRadio(center,pos,radiu))
                {
                    cameraList.Add(item.id);
                }
            }
            return Ok(new {
                res=cameraList
            });

        }

        /// <summary>
        /// 获取摄像机一定圆形范围内的所有摄像头list
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCameraWorldRangeList/{wordX}/{wordY}/{radiu}")]
        public IActionResult GetCameraWorldRangeList(string wordX, string wordY, float radiu)
        {
            if (string.IsNullOrEmpty(wordX) || string.IsNullOrEmpty(wordY))
            {
                return BadRequest();
            }

            float.TryParse(wordX, out float centerX);
            float.TryParse(wordY, out float centerY);

            System.Numerics.Vector3 center = new System.Numerics.Vector3(centerX, centerY, 0);
            //距离小于等于半径
            List<sys_cameras> query = _cameraRepo.FindList(p => true, "", false).ToList<sys_cameras>();

            List<string> cameraList = new List<string>();
            for (int i = 0; i < query.Count; i++)
            {
                var item = query[i];
                float.TryParse(item.worldX,out float x);
                float.TryParse(item.worldY, out float y);

                System.Numerics.Vector3 pos = new System.Numerics.Vector3(x, y, 0);
                if (CheckInRadio(center,pos,radiu))
                {
                    cameraList.Add(item.id);
                }
            }
            return Ok(new {
                res=cameraList
            });

        }





        private bool CheckInRadio(System.Numerics.Vector3 center, System.Numerics.Vector3 camPos,float radiu) {

            float distance = System.Numerics.Vector3.Distance(center, camPos);
            if (distance<=radiu)
            {
                return true;
            }
            return false;
        }


    }
}
