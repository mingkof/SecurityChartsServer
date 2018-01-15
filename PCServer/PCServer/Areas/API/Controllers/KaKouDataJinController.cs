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

namespace SHSecurityServer.Controllers
{
    [Produces("application/json")]
    [Route("api/kakoudatajin")]
    public class KaKouDataJinController : Controller
    {
        private readonly ILogger _logger;
        // private readonly IKaKouDataHistoryJinRepository _kakoudatajin_history;
        private readonly IKaKouDataJinRepository _kakoudatajin;
        private readonly IKaKouTopRepository _kakouTop;

        public KaKouDataJinController(IKaKouDataJinRepository kakoudata, IKaKouTopRepository kakouTop, ILogger<Sys110WarnController> logger)
        {
            _logger = logger;
            _kakoudatajin = kakoudata;
            _kakouTop = kakouTop;
        }

        [HttpGet("getkakoudata/")]
        public IActionResult GetKaKouData()
        {
            var query=_kakoudatajin.FindList(p => true,"",false);
            if (query!=null)
            {
                return Ok(new {
                    res=query
                });
            }else
            {
                return BadRequest();
            }
        }
        /// <summary>
        /// 获取卡口流量top 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetKakouTop5Data/")]
        public IActionResult GetKakouTop5Data()
        {
            var query = _kakouTop.FindList(p => true, "", false);
            if (query!=null)
            {

                return Ok(new {
                    res = query
                });
            }
            return BadRequest();
        }

    }
 }