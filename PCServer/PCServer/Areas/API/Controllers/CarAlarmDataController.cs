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
    [Route("api/CarAlarmData")]
    public class CarAlarmDataController : Controller
    {
        private readonly ILogger _logger;
        private readonly ICarAlarmDataRepositoy _carAlarmData;
        private readonly ISysTicketresRepository _sysTicketresRepository;

        private readonly IHostingEnvironment _hostingEnv;
        private RealDataUrl RealDataUrlConfig;
        public CarAlarmDataController(ICarAlarmDataRepositoy carAlarmData, ISysTicketresRepository sysTicketresRepository, IHostingEnvironment hostingEnv, ILogger<FaceAlarmDataController> logger, IOptions<RealDataUrl> config)
        {
            _logger = logger;
            _carAlarmData = carAlarmData;
            _sysTicketresRepository = sysTicketresRepository;
            RealDataUrlConfig = config.Value;
            _hostingEnv = hostingEnv;
        }
        /// <summary>
        /// 获取车脸识别列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("GetCarAlarmPageList/{pageIndex}/{pageSize}")]
        public IActionResult GetCarAlarmPageList(int pageIndex,int pageSize)
        {
            var query = _carAlarmData.FindPageList(pageIndex, pageSize, out int totalSize, p => true, "", false);
            if (query!=null)
            {
                return Ok(new {
                    res=query
                });

            }

            return BadRequest();
        }


    }
 } 

