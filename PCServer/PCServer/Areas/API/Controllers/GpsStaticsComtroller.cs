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

namespace SHSecurityServer.Controllers
{
    class GpsStructA
    {
        public string x { get; set; }
        public string y { get; set; }
        public int count { get; set; }
    }


    [Produces("application/json")]
    [Route("api/gpstatics")]
    public class GpsStaticsComtroller : Controller
    {
        private readonly ILogger _logger;
        private readonly ISys110WarningRepository _sys110warnRepository;
        private readonly IPoliceGpsRepository _policeGps;
        private readonly IGpsGridRepository _gpsGrid;
        public GpsStaticsComtroller(ISys110WarningRepository sys110warnRepos, IPoliceGpsRepository policeGps, IGpsGridRepository gpsGrid, ILogger<GpsStaticsComtroller> logger)
        {
            _logger = logger;
            _policeGps = policeGps;
            _gpsGrid = gpsGrid;
            _sys110warnRepository = sys110warnRepos;
        }

        //[HttpGet("test")]
        //public IActionResult Test()
        //{
        //    var query = _sys110warnRepository.Find(p => p.JJD_ID == "20171204110502518099");
        //    var gps_x = query.AMAP_GPS_X;
        //    var gps_y = query.AMAP_GPS_Y;

        //    Vector3 vec = GPSUtils.ComputeLocalPositionGCJ(gps_x, gps_y);

        //    int newX = (int)(vec.x / 100);
        //    int newY = (int)(vec.y / 100);

        //    string str = newX + "   " + newY + "   " + vec.z;

        //    return Ok(new
        //    {
        //        res = str
        //    });
        //}

        [HttpGet("GetCount")]
        public IActionResult GetCount(int TimeStart, int TimeEnd) {

            var queryList = _gpsGrid.FindList(p => p.JJD_TIMESIGN >= TimeStart && p.JJD_TIMESIGN <= TimeEnd, "", false);

            var group = queryList.GroupBy(p => p.GridName);

            Dictionary<string, int> res = new Dictionary<string, int>();
            foreach (var item in group)
            {
                var Key = item.Key;

                foreach (var detail in item)
                {
                    if (res.ContainsKey(Key))
                    {
                        ++res[Key];
                    }
                    else
                    {
                        res.Add(Key, 1);
                    }
                }
            }

            List<GpsStructA> list = new List<GpsStructA>();
            foreach (var item in res)
            {
                var arr = item.Key.Split('_');
                list.Add(new GpsStructA()
                {
                    x = arr[0],
                    y = arr[1],
                    count = item.Value
                });
            }

            return Ok(new
            {
                res = list
            });
        }

    }
}
