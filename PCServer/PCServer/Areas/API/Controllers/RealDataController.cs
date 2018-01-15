using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SHSecurityContext.IRepositorys;
using SHSecurityModels;
using System.Text;
using KVDDDCore.Utils;
using Microsoft.Extensions.Options;
using MKServerWeb.Model.RealData;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace SHSecurityServer.Controllers
{
    [Produces("application/json")]
    [Route("api/realdata")]
    public class RealDataController : Controller
    {
        private readonly ILogger _logger;
        private RealDataUrl RealDataUrlConfig;


        public RealDataController(ILogger<RealDataController> logger, IOptions<RealDataUrl> config)
        {
            _logger = logger;
            RealDataUrlConfig = config.Value;
        }

        [HttpGet("weather", Name = "GetWeatherData")]
        public IActionResult GetWeatherData()
        {

            var model = WebClientUls.GetString(RealDataUrlConfig.WeatherUrl);
            WeartherData weatherData;
            if (model != null)
            {
                weatherData = new WeartherData
                {
                    AirQuality = model["airQuality"].ToString(),
                    BigTemperature = int.Parse(model["bigTemperature"].ToString()),
                    Humidity = int.Parse(model["humidity"].ToString()),
                    SmallTemperature = int.Parse(model["smallTemperature"].ToString()),
                    Weather = model["weather"].ToString(),
                    Wind = model["wind"].ToString()
                };
            }
            else
            {
                weatherData = new WeartherData
                {
                    AirQuality = "良",
                    BigTemperature = 17,
                    Humidity = 43,
                    SmallTemperature = 12,
                    Weather = "多云",
                    Wind = "北风"
                };
            }
            return Ok(weatherData);
        }

        [HttpGet("traffic", Name = "GetTrafficData")]
        public IActionResult GetTrafficData()
        {
            var model = WebClientUls.GetString(RealDataUrlConfig.TrafficUrl);
            var modelRoadTop = WebClientUls.GetString(RealDataUrlConfig.RoadUrl);

            TrafficData old_tampData = null;
            TrafficData tampData = null;



            if (model != null)
            {
                _logger.LogInformation(model["data"]["overview"]["traIndex"].ToString());

                tampData = new TrafficData
                {
                    TrafficDataForAll = model["data"]["overview"]["traIndex"].ToString(),
                    TrafficAvgSpeed = model["data"]["overview"]["avgSpeed"].ToString(),
                    TopsRoads = new TrafficRoadState[5]
                };
            }
            if (modelRoadTop != null)
            {
                for (int i = 0; i < 5; i++)
                {
                    tampData.TopsRoads[i] = new TrafficRoadState(
                            modelRoadTop["data"]["rows"][i]["roadName"].ToString(),
                            modelRoadTop["data"]["rows"][i]["speed"].ToString(),
                            modelRoadTop["data"]["rows"][i]["traIndex"].ToString());
                }
            }
            if (tampData == null)
            {
                //测试数据
                if (old_tampData == null)
                {
                    return Ok(new JsonRoadDataStruct
                    {
                        TopRoads = new List<JsonRoadItemStruct>(){
                        new JsonRoadItemStruct {
                            RoadName="天目西路",
                            TrafficAvgSpeed="35",
                            TrafficData="2.5"
                        },
                        new JsonRoadItemStruct{
                             RoadName="大统路",
                            TrafficAvgSpeed="45",
                            TrafficData="2.0"
                        },
                        new JsonRoadItemStruct{
                             RoadName="中兴路",
                            TrafficAvgSpeed="38",
                            TrafficData="2.8"
                        },
                        new JsonRoadItemStruct{
                             RoadName="恒丰路",
                            TrafficAvgSpeed="15",
                            TrafficData="3.8"
                        }
                        },
                        TrafficDataForAll = "3.2",
                        TrafficAvgSpeed = "30",

                    });
                }
                return Ok(old_tampData);
            }
            else
            {
                old_tampData = tampData;
                return Ok(tampData);
            }

            //_logger.LogInformation(tampData.TrafficAvgSpeed);
            //return Ok(tampData);
            //}
        }

        [HttpGet("ftp/{path}", Name = "FtpTest")]
        public IActionResult FtpTest(string path)
        {
            FtpClient ftpClient = new FtpClient(RealDataUrlConfig.ip, RealDataUrlConfig.username, RealDataUrlConfig.userpassword);

            var res = ftpClient.Download(path);

            if (res != null)
                return Ok(res);
            else
                return Ok("数据为空");
        }
    }

    //public class RealDataUrl
    //{
    //    public string WeatherUrl { get; set; }

    //    public string TrafficUrl { get; set; }

    //    public string RoadUrl { get; set; }
    //}

    //public class WeartherData
    //{
    //    public string AirQuality { get; set; }

    //    public int BigTemperature { get; set; }

    //    public int Humidity { get; set; }

    //    public int SmallTemperature { get; set; }

    //    public string Weather { get; set; }

    //    public string Wind { get; set; }

    //}

    //public class TrafficData
    //{
    //    public string TrafficDataForAll { get; set; }

    //    public string TrafficAvgSpeed { get; set; }

    //    public TrafficRoadState[] TopsRoads;
    //}

    //public class TrafficRoadState
    //{
    //    public string RoadName { get; set; }

    //    public string TrafficAvgSpeed { get; set; }

    //    public string TrafficData { get; set; }

    //    public TrafficRoadState(string roadName, string trafficAvgSpeed, string trafficData)
    //    {
    //        RoadName = roadName;
    //        TrafficAvgSpeed = trafficAvgSpeed;
    //        TrafficData = trafficData;
    //    }
    //}
}