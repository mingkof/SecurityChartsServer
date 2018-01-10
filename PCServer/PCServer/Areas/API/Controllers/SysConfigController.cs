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
    [Route("api/sysconfig")]
    public class SysConfigController : Controller
    {
        private readonly ILogger _logger;
        private readonly ISysConfigRepository _sysConfig;
        public SysConfigController(ISysConfigRepository sysConfig,ILogger<Sys110WarnController> logger)
        {
            _logger = logger;
            _sysConfig = sysConfig;
        }

        [HttpGet("ChangeSipStatus/{value}")]
        public IActionResult ChangeSipStatus(int value)
        {
            int key=(int)EConfigKey.kSipSCStatus;
            var query=_sysConfig.Find(p=>p.key==key);
            if (query==null)
            {
                _sysConfig.Add(new sys_config(){
                    key=key,
                    value="",
                    valueInt=value
                });
            }
            else
            {
                query.valueInt=value;
                _sysConfig.Update(query);
            }
            return Ok("ok");
        }
        [HttpGet("GetSipStatus/")]
        public IActionResult GetSipStatus()
        {
            int key=(int)EConfigKey.kSipSCStatus;
            var query=_sysConfig.Find(p=>p.key==key);
            if (query==null)
            {
                 _sysConfig.Add(new sys_config(){
                    key=key,
                    value="",
                    valueInt=0
                });
                return Ok(new {
                    res=0
                });
            }
            else
            {
                return Ok(new {
                    res=query.valueInt
                });
            }
        }
    }
 }