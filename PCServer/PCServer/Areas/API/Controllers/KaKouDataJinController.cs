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
        public KaKouDataJinController(IKaKouDataJinRepository kakoudata,ILogger<Sys110WarnController> logger)
        {
            _logger = logger;
            _kakoudatajin = kakoudata;
            // _kakoudatajin_history = kakoudata_history;
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
                // return Ok("1111");
            }else
            {
                return BadRequest("2222");
            }
            // return Ok("1111");
        }
    }
 }