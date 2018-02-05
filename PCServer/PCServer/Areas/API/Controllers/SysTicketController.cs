using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SHSecurityContext.IRepositorys;
using SHSecurityModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using KVDDDCore.Utils;

namespace MKServerWeb.Controllers
{
    /// <summary>
    /// 购票报警信息
    /// </summary>
    [Produces("application/json")]
    [Route("api/ticket")]
    public class SysTicketController : Controller
    {
        private readonly ILogger _logger;
        private readonly ISysTicketresRepository _sysTicketresRepository;
        public SysTicketController(ISysTicketresRepository sysTicketresRepository, ILogger<SysTicketController> logger)
        {
            _logger = logger;

            _sysTicketresRepository = sysTicketresRepository;
        }

        /// <summary>
        /// 获取历史所有购票报警记录数量
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetHistoryCount")]
        public IActionResult GetHistoryCount()
        {
            var count = _sysTicketresRepository.Count(p => true);
            return Ok(new
            {
                res = count
            });
        }
        /// <summary>
        /// 获取历史所有购票报警记录列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("list", Name = "TicketList")]
        public IActionResult Get()
        {
            var list = _sysTicketresRepository.FindList(p => true, "", false);

            if (list == null)
                return BadRequest("无数据");

            return Ok(new
            {
                array = list
            });
        }
        /// <summary>
        /// 获取今日所有购票报警信息列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetTodayList")]
        public IActionResult GetTodayList()
        {
            //string today = DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00";
            //int todayStamp = TimeUtils.ConvertToTimeStamps(today);
            string nowYear = System.DateTime.Now.Year.ToString();
            string nowMonth = System.DateTime.Now.Month.ToString("00");
            string nowDay = System.DateTime.Now.Day.ToString("00");

            var query = _sysTicketresRepository.FindList(p => p.TicketYear == nowYear && p.TicketMonth == nowMonth && p.TicketDay == nowDay, "", false);

            return Ok(new {
                res=query
            });
        }
        /// <summary>
        /// 分页获取今日购票报警信息列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("GetToadyPageList/{pageIndex}/{pageSize}")]
        public IActionResult GetToadyPageList(int pageIndex,int pageSize)
        {
            //string today = DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00";
            //int todayStamp = TimeUtils.ConvertToTimeStamps(today);

            string nowYear = System.DateTime.Now.Year.ToString();
            string nowMonth = System.DateTime.Now.Month.ToString("00");
            string nowDay = System.DateTime.Now.Day.ToString("00");


            var query = _sysTicketresRepository.FindPageList(pageIndex, pageSize, out int totalSize, p => p.GoDateYear == nowYear && p.GoDateMonth == nowMonth && p.GoDateDay == nowDay, "GoTime", false);

            return Ok(new {
                res=query
            });
        }
        /// <summary>
        /// 获取今日购票报警次数
        /// </summary>
        /// <returns>res=count</returns>
        [HttpGet("GetToadyCount")]
        public IActionResult GetToadyCount()
        {
            //string today = DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00";
            //int todayStamp = TimeUtils.ConvertToTimeStamps(today);

            string nowYear = System.DateTime.Now.Year.ToString();
            string nowMonth = System.DateTime.Now.Month.ToString("00");
            string nowDay = System.DateTime.Now.Day.ToString("00");

            var query = _sysTicketresRepository.Count(p => p.TicketYear == nowYear && p.TicketMonth == nowMonth && p.TicketDay == nowDay);

            return Ok(new
            {
                res = query
            });
        }

    }
}